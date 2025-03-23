using System.Collections.Immutable;
using System.Text.Json;
using TOrgName.TIamAzureRepositoryName.Pulumi.Contract;
using TOrgName.TIamAzureRepositoryName.Pulumi.Contract.OutputTypes;
using TOrgName.TIamAzureRepositoryName.Pulumi.Extensions;
using TOrgName.TPlatformPulumiRepositoryName.Core.Helpers;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.RoleAssignments;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Identities.WorkloadIdentities;
using TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Mappers;
using TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceGroups;
using TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceProviders;
using TOrgName.TIamAzureRepositoryName.Pulumi.StackReferences.TIamEntraIdRepositoryNameStackReference.Mappers;
using ResourceGroupType = TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceGroups.ResourceGroupType;

namespace TOrgName.TIamAzureRepositoryName.Pulumi;

internal sealed class MainStack : Stack
{
    public MainStack()
    {
        StackReference iamEntraidMainStack = StackReferenceHelper.GetStackReference(
            TIamEntraIdRepositoryName.Pulumi.Contract.StackDetails.Organization,
            TIamEntraIdRepositoryName.Pulumi.Contract.StackDetails.Project, 
            TIamEntraIdRepositoryName.Pulumi.Contract.StackNames.Main);

        var resourceProviders = new ResourceProviders(ResourceNamingConvention.GetResourceName(
                PulumiResourceType.ComponentResource, "resource-providers"));
        
        ImmutableDictionary<ApplicationIdentityType, ApplicationIdentity> applicationIdentities = TIamEntraIdRepositoryNameStackOutputMapper.GetApplicationIdentitiesAsync(iamEntraidMainStack)
            .GetAwaiter().GetResult();

        RoleAssignment subscriptionOwner = AssignSelfAsSubscriptionOwner(resourceProviders, applicationIdentities);

        var resourceGroups = new ResourceGroups(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "resource-groups"), 
            applicationIdentities,
            new ComponentResourceOptions
            {
                DependsOn = { resourceProviders, subscriptionOwner }
            });
        
        ResourceGroups = resourceGroups.ResourceGroupMap.Apply(map =>
            Output.All(map.Select(kvp =>
                Output.Tuple(
                    kvp.Value.ResourceGroupName,
                    kvp.Value.ManagedIdentityId,
                    kvp.Value.ManagedIdentityClientId,
                    kvp.Value.ManagedIdentityPrincipalId
                ).Apply(tuple =>
                {
                    (string rgName, string miId, string miClientId, string miPrincipalId) = tuple;
        
                    string key = ResourceGroupContractMapper
                        .Map(ResourceGroupType.FromName(kvp.Key))
                        .ToString();
                    string value = JsonSerializer.Serialize(new ResourceGroup(rgName, miId, miClientId, miPrincipalId));
                    return KeyValuePair.Create(key, value);
                })
            )).Apply(list => list.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value)));
    }
    
    [Output(StackOutputs.ResourceGroups)]
    public Output<ImmutableDictionary<string, string>> ResourceGroups { get; set; }
    
    private static RoleAssignment AssignSelfAsSubscriptionOwner(ResourceProviders resourceProviders,
        ImmutableDictionary<ApplicationIdentityType, ApplicationIdentity> applicationIdentities)
    {
        Output<GetClientConfigResult> azureClientConfig = GetClientConfig.Invoke(new()
        {
            DependsOn = { resourceProviders }
        });

        ApplicationIdentity iamAzureApplicationIdentity = applicationIdentities.GetValueOrThrow(ApplicationIdentityType.TIamAzureRepositoryName);
        
        return new RoleAssignmentBuilder(ResourceNamingConvention.GetResourceName(AzureResourceType.RoleAssignment, "TIamAzureRepositoryGitLabPath-spowner"),
                iamAzureApplicationIdentity.ServicePrincipalObjectId, PrincipalType.ServicePrincipal, AzureRoleType.Owner, 
                azureClientConfig.Apply(x => $"/subscriptions/{x.SubscriptionId}"))
            .Build();
    }
}
