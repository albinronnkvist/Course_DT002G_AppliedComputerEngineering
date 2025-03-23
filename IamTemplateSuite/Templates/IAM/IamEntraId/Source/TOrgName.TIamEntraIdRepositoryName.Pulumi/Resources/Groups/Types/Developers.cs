using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.HumanIdentities;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Groups.Types;

internal class Developers : ComponentResource, IGroup
{
    public Developers(string resourceName, Output<Dictionary<string, DirectoryRole>> directoryRoles,
        Output<Dictionary<string, User>> users, ComponentResourceOptions? options = null) 
        : base("custom:azuread:Developers", resourceName, options)
    {
        Group = users.Apply(userMap =>
        {
            var owners = new List<User>
            {
                userMap[UserIdentityType.AlbinRonnkvist.Name]
            };
            
            var members = new List<User>
            {
                userMap[UserIdentityType.PiranAmedi.Name]
            };
            
            GroupBuilder group = new GroupBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.Group, GroupType.Developers.Name), 
                    GroupType.Developers, new CustomResourceOptions { Parent = this })
                .WithDescription("Developers group")
                .WithOwners(owners)
                .WithMembers(members);
            
            return group.Build();
        });
        
        RegisterOutputs(new Dictionary<string, object?>
        {
            { "group", Group }
        });
    }
    
    public Output<Group> Group { get; }
}
