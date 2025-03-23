using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Groups.Types;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Groups;

internal class Groups : ComponentResource
{
    public Groups(string resourceName, Output<Dictionary<string, DirectoryRole>> directoryRoles, 
        Output<Dictionary<string, User>> users, ComponentResourceOptions? options = null) 
        : base("custom:azuread:Groups", resourceName, options)
    {
        var groupInstances = new Dictionary<string, Output<Group>>
        {
            { GroupType.Developers.Name, new Developers(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, GroupType.Developers.Name), 
                    directoryRoles, users, new ComponentResourceOptions { Parent = this })
                .Group 
            }
        };
        
        GroupMap = Output.All(groupInstances.Values.ToArray())
            .Apply(groups =>
                groupInstances.Keys.Zip(groups, (key, group) => new { key, group })
                    .ToDictionary(x => x.key, x => x.group));

        RegisterOutputs(new Dictionary<string, object?>
        {
            { "groupMap", GroupMap }
        });
    }
    
    public Output<Dictionary<string, Group>> GroupMap { get; }
}
