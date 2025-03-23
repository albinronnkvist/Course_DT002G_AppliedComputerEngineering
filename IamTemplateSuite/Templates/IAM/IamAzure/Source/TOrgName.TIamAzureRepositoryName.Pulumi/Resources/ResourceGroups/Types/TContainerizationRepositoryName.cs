using TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.RoleAssignments;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.ManagedIdentity;
using Pulumi.AzureNative.Resources;
using TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Identities.WorkloadIdentities;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceGroups.Types;

internal sealed class TContainerizationRepositoryName : ComponentResource, IResourceGroup
{
    public TContainerizationRepositoryName(string resourceName, ApplicationIdentity applicationIdentity, ComponentResourceOptions options)
        : base("custom:azurenative:TContainerizationRepositoryName", resourceName, options)
    {
        ResourceGroupType resourceGroupType = ResourceGroupType.TContainerizationRepositoryName;
        
        var resourceGroup = new ResourceGroup(ResourceNamingConvention.GetResourceName(AzureResourceType.ResourceGroup, resourceGroupType.WorkloadName), 
            options: new CustomResourceOptions
        {
            DependsOn = options.DependsOn,
            Parent = this
        });
        
        var managedIdentity = new UserAssignedIdentity(ResourceNamingConvention.GetResourceName(AzureResourceType.UserAssignedManagedIdentity, resourceGroupType.WorkloadName), 
            new UserAssignedIdentityArgs
        {
            ResourceGroupName = resourceGroup.Name
        }, new CustomResourceOptions{ Parent = this });
        
        _ = new RoleAssignmentBuilder(ResourceNamingConvention.GetResourceName(AzureResourceType.RoleAssignment, $"{resourceGroupType.WorkloadName}-spowner"), 
                applicationIdentity.ServicePrincipalObjectId, PrincipalType.ServicePrincipal, AzureRoleType.Owner, resourceGroup.Id, 
                new CustomResourceOptions{ Parent = this })
            .Build();
        
        // TODO: assign roles to Groups (paid feature)
        
        ResourceGroupName = resourceGroup.Name;
        ManagedIdentityId = managedIdentity.Id;
        ManagedIdentityClientId = managedIdentity.ClientId;
        ManagedIdentityPrincipalId = managedIdentity.PrincipalId;
        RegisterOutputs(new Dictionary<string, object?>
        {
            { "resourceGroup", ResourceGroupName },
            { "managedIdentityId", ManagedIdentityId },
            { "managedIdentityClientId", ManagedIdentityClientId },
            { "managedIdentityPrincipalId", ManagedIdentityPrincipalId }
        });
    }
    
    public Output<string> ResourceGroupName { get; }
    public Output<string> ManagedIdentityId { get; }
    public Output<string> ManagedIdentityClientId { get; }
    public Output<string> ManagedIdentityPrincipalId { get; }
}
