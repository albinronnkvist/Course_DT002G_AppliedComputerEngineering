using System.Collections.Immutable;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.DirectoryRoles;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Domains;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.HumanIdentities;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureAD;
using Shouldly;
using Xunit;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Test;

public sealed partial class MainStackTests
{
    [Fact]
    public async Task UserIdentitiesComponentResourceShouldExist()
    {
        ImmutableArray<Resource> resources = await TestAsync();

        resources.OfType<ComponentResource>()
            .Single(x => 
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "user-identities"), StringComparison.Ordinal))
            .ShouldNotBeNull();
    }
    
    [Fact]
    public async Task UserIdentitiesShouldContainExpectedUserIdentityTypes()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        var users = resources.OfType<User>().ToList();
        
        UserData[] userDataList = await Task.WhenAll(users.Select(async user => new UserData
        {
            DisplayName = await user.DisplayName.GetValueAsync(),
            UserPrincipalName = await user.UserPrincipalName.GetValueAsync(),
            ForcePasswordChange = await user.ForcePasswordChange.GetValueAsync()
        }));
        
        var expectedDisplayNames = UserIdentityType.List.Select(user => user.DisplayName).ToList();
        var expectedUserPrincipalNames =
            UserIdentityType.List.Select(user => $"{user.Name}@{DomainType.Default.Name}").ToList();

        userDataList.Select(u => u.DisplayName).ShouldBe(expectedDisplayNames, true);
        userDataList.Select(u => u.UserPrincipalName).ShouldBe(expectedUserPrincipalNames, true);
        userDataList.ShouldAllBe(u => u.ForcePasswordChange.HasValue && u.ForcePasswordChange.Value);
    }
    
    [Fact]
    public async Task UserIdentitiesShouldHaveExpectedDirectoryRoleAssignments()
    {
        ImmutableArray<Resource> resources = await TestAsync();

        var directoryRoleAssignments = new List<DirectoryRoleAssignment?>();
        foreach (UserIdentityType userType in UserIdentityType.List.Where(user => user.DirectoryRoles.Count > 0))
        {
            User user = GetUserIdentity(resources, userType);
            foreach (DirectoryRoleType role in userType.DirectoryRoles)
            {
                DirectoryRoleAssignment directoryRoleAssignment = GetUserIdentityDirectoryRoleAssignment(resources, role, await user.ObjectId.GetValueAsync());
                directoryRoleAssignments.Add(directoryRoleAssignment);
            }
        }
        
        directoryRoleAssignments.ShouldAllBe(x => x != null);
    }
    
    private static User GetUserIdentity(ImmutableArray<Resource> resources, UserIdentityType userIdentityType) =>
         resources
            .OfType<User>()
            .Single(x => x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(EntraIdResourceType.User, userIdentityType.Name), StringComparison.Ordinal));
    
    private static DirectoryRoleAssignment GetUserIdentityDirectoryRoleAssignment(ImmutableArray<Resource> resources, DirectoryRoleType directoryRoleType, string userObjectId) =>
        resources
            .OfType<DirectoryRoleAssignment>()
            .Single(x => x.GetResourceName()
                .Equals(ResourceNamingConvention.GetResourceName(EntraIdResourceType.DirectoryRoleAssignment, 
                    $"{directoryRoleType.TemplateId}-{userObjectId}"), StringComparison.Ordinal));
    
    private sealed record UserData
    {
        public string DisplayName { get; init; } = null!;
        public string UserPrincipalName { get; init; } = null!;
        public bool? ForcePasswordChange { get; init; }
    }
}
