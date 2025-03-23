using System.Collections.Immutable;
using System.Text.Json;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Mappers;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.DirectoryRoles;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Groups;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.HumanIdentities;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi;

internal class MainStack : Stack
{
    public MainStack()
    {
        var directoryRoles = new DirectoryRoles(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "directory-roles"));
        
        var userIdentities = new UserIdentities(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "user-identities"), 
            directoryRoles.DirectoryRoleMap);
        
        var applicationIdentities = new ApplicationIdentities(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "application-identities"), 
            directoryRoles.DirectoryRoleMap);
        
        _ = new Groups(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "groups"), 
            directoryRoles.DirectoryRoleMap, userIdentities.UserMap);
        
        ApplicationIdentities = applicationIdentities.ApplicationIdentityMap.Apply(map =>
            Output.All(map.Select(kvp =>
                Output.Tuple(
                    kvp.Value.ApplicationClientId,
                    kvp.Value.ServicePrincipalObjectId
                ).Apply(tuple =>
                {
                    (string appId, string spId) = tuple;

                    string key = ApplicationIdentityTypeContractMapper
                        .Map(ApplicationIdentityType.FromName(kvp.Key))
                        .ToString();
                    string value = JsonSerializer.Serialize(new ApplicationIdentity(appId, spId));
                    return KeyValuePair.Create(key, value);
                })
            )).Apply(list => list.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value)));
    }
    
    [Output(StackOutputs.ApplicationIdentities)]
    public Output<ImmutableDictionary<string, string>> ApplicationIdentities { get; set; }
}
