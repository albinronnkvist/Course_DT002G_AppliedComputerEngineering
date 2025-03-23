using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.DirectoryRoles;

internal class DirectoryRoles : ComponentResource
{
    public Output<Dictionary<string, DirectoryRole>> DirectoryRoleMap { get; }
    
    public DirectoryRoles(string resourceName, ComponentResourceOptions? options = null)
        : base("custom:azuread:DirectoryRoles", resourceName, options)
    {
        var roles = DirectoryRoleType.List
            .ToDictionary(role => role.Name, role => 
                new DirectoryRoleBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.DirectoryRole, role.Name), 
                        role, new CustomResourceOptions { Parent = this })
                    .Build());
        
        DirectoryRoleMap = Output.Create(roles);
        RegisterOutputs(new Dictionary<string, object?>
        {
            { "directoryRoleMap", DirectoryRoleMap }
        });
    }
}
