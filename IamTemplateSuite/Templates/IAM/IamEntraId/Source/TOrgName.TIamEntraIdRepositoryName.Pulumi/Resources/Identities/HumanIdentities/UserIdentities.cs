using System.Collections.ObjectModel;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.HumanIdentities;

internal class UserIdentities : ComponentResource
{
    public UserIdentities(string resourceName, Output<Dictionary<string, DirectoryRole>> directoryRoles, ComponentResourceOptions? options = null) 
        : base("custom:azuread:UserIdentities", resourceName, options)
    {
        var users = Output.Create(UserIdentityType.List
            .ToDictionary(user => user.Name, user => 
                new UserBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.User, user.Name), 
                        user, new CustomResourceOptions { Parent = this })
                    .Build()));
        
        AssignDirectoryRoles(directoryRoles, users);
        
        UserMap = users;
        RegisterOutputs(new Dictionary<string, object?>
        {
            { "userMap", UserMap }
        });
    }
    
    public Output<Dictionary<string, User>> UserMap { get; }

    private static void AssignDirectoryRoles(Output<Dictionary<string, DirectoryRole>> directoryRoles, Output<Dictionary<string, User>> users) =>
        Output.Tuple(directoryRoles, users)
            .Apply(rolesAndUsersMaps =>
            {
                (Dictionary<string, DirectoryRole> rolesMap, Dictionary<string, User> entitiesMap) = rolesAndUsersMaps;
            
                foreach (UserIdentityType userType in UserIdentityType.List)
                {
                    if (!entitiesMap.TryGetValue(userType.Name, out User? user))
                    {
                        continue;
                    }
            
                    ReadOnlyCollection<DirectoryRole> userDirectoryRoles = userType.DirectoryRoles
                        .Select(roleType => rolesMap[roleType.Name])
                        .ToList()
                        .AsReadOnly();
            
                    foreach (DirectoryRole directoryRole in userDirectoryRoles)
                    {
                        Output.Tuple(directoryRole.TemplateId, user.ObjectId).Apply(roleIdAndUserIdMap =>
                        {
                            (string directoryRoleTemplateId, string userObjectId) = roleIdAndUserIdMap;
                        
                            new DirectoryRoleAssignmentBuilder(
                                    ResourceNamingConvention.GetResourceName(EntraIdResourceType.DirectoryRoleAssignment, $"{directoryRoleTemplateId}-{userObjectId}"), 
                                    directoryRole.TemplateId, 
                                    user.ObjectId)
                                .Build();
                        
                            return roleIdAndUserIdMap;
                        });
                    }
                }
            
                return rolesAndUsersMaps;
            });
}
