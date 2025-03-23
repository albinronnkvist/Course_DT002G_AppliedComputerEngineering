using System.Collections.Immutable;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Groups;
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
    public async Task GroupsComponentResourceShouldExist()
    {
        ImmutableArray<Resource> resources = await TestAsync();

        resources.OfType<ComponentResource>()
            .Single(x => 
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "groups"), StringComparison.Ordinal))
            .ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GroupsShouldContainExpectedGroups()
    {
        ImmutableArray<Resource> resources = await TestAsync();

        var groups = resources.OfType<Group>().ToList();
        string[] groupNames = await Task.WhenAll(groups.Select(async group => await group.DisplayName.GetValueAsync()));
        
        groupNames.ShouldBe(GroupType.List
                .Select(group => group.DisplayName)
                .ToList(),
            true
        );
    }

    [Fact]
    public async Task DevelopersGroupShouldHaveExpectedSetup()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        GroupType groupType = GroupType.Developers;
        
        ComponentResource componentResource = GetGroupComponentResource(resources, groupType);
        Group group = GetGroup(resources, groupType);

        componentResource.ShouldNotBeNull();
        (await group.DisplayName.GetValueAsync()).ShouldBe(groupType.DisplayName);
        (await group.Description.GetValueAsync()).ShouldBe("Developers group");
        (await group.Owners.GetValueAsync()).ShouldBe(new []
        {
            await GetUserIdentity(resources, UserIdentityType.AlbinRonnkvist).ObjectId.GetValueAsync()
        });
        (await group.Members.GetValueAsync()).ShouldBe(new []
        {
            await GetUserIdentity(resources, UserIdentityType.PiranAmedi).ObjectId.GetValueAsync()
        });
    }
    
    private static ComponentResource GetGroupComponentResource(ImmutableArray<Resource> resources, GroupType groupType) =>
        resources
            .OfType<ComponentResource>()
            .Single(x =>
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, groupType.Name), StringComparison.Ordinal));

    private static Group GetGroup(ImmutableArray<Resource> resources, GroupType groupType) =>
        resources
            .OfType<Group>()
            .Single(x =>
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(EntraIdResourceType.Group, groupType.Name), StringComparison.Ordinal));
}
