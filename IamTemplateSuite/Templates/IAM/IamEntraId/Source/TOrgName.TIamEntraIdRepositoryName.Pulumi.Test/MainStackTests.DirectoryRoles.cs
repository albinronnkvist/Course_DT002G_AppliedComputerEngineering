using System.Collections.Immutable;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.DirectoryRoles;
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
    public async Task DirectoryRolesComponentResourceShouldExist()
    {
        ImmutableArray<Resource> resources = await TestAsync();

        resources.OfType<ComponentResource>()
            .Single(x => 
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "directory-roles"), StringComparison.Ordinal))
            .ShouldNotBeNull();
    }
    
    [Fact]
    public async Task DirectoryRolesShouldContainExpectedRoles()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        
        var directoryRoles = resources.OfType<DirectoryRole>().ToList();
        string[] templateIds = await Task.WhenAll(directoryRoles
            .Select(async role => await role.TemplateId.GetValueAsync()));
        
        templateIds.ShouldBe(
            DirectoryRoleType.List
                .Select(role => role.TemplateId)
                .ToList(),
            true
        );
    }
}
