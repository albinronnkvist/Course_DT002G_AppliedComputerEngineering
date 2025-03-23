using System.Collections.Immutable;
using TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceGroups;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.RoleAssignments;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.ManagedIdentity;
using Pulumi.AzureNative.Resources;
using Shouldly;
using Resource = Pulumi.Resource;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Test;

public sealed partial class MainStackTests
{
    [Fact]
    public async Task ResourceGroupsComponentResourceShouldExist()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        
        resources.OfType<ComponentResource>()
            .Single(x => 
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "resource-groups"), StringComparison.Ordinal))
            .ShouldNotBeNull();
    }
    
    [Fact]
    public async Task ShouldCreateExpectedResourceGroupComponentResources()
    {
        ImmutableArray<Resource> resources = await TestAsync();
    
        var componentResourcesNames = resources.OfType<ComponentResource>()
            .Select(resourceProvider => resourceProvider.GetResourceName())
            .ToList();

        var resourceGroupComponentResourcesNames = ResourceGroupType.List
            .Select(resourceProvider =>
                ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, resourceProvider.Name))
            .ToList();
        
        resourceGroupComponentResourcesNames.ShouldBeSubsetOf(componentResourcesNames);
    }
    
    [Fact]
    public async Task TContainerizationRepositoryNameResourceGroupShouldHaveExpectedSetup()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        ResourceGroupType type = ResourceGroupType.TContainerizationRepositoryName;
        ResourceGroup rg = GetResourceGroup(resources, type);
        UserAssignedIdentity mi = GetManagedIdentity(resources, type);
        RoleAssignment ra = GetRoleAssignment(resources, type, "spowner");

        rg.ShouldNotBeNull();
        mi.ShouldNotBeNull();
        (await ra.RoleDefinitionId.GetValueAsync()).ShouldBe(AzureRoleType.Owner.RoleDefinitionId);
        (await ra.PrincipalType.GetValueAsync()).ShouldBe(PrincipalType.ServicePrincipal.ToString());
    }

    [Fact]
    public async Task TMicroserviceRepositoryNameResourceGroupShouldHaveExpectedSetup()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        ResourceGroupType type = ResourceGroupType.TMicroserviceRepositoryName;
        ResourceGroup rg = GetResourceGroup(resources, type);
        UserAssignedIdentity mi = GetManagedIdentity(resources, type);
        RoleAssignment ra = GetRoleAssignment(resources, type, "spowner");

        rg.ShouldNotBeNull();
        mi.ShouldNotBeNull();
        (await ra.RoleDefinitionId.GetValueAsync()).ShouldBe(AzureRoleType.Owner.RoleDefinitionId);
        (await ra.PrincipalType.GetValueAsync()).ShouldBe(PrincipalType.ServicePrincipal.ToString());
    }
    
    private static ResourceGroup GetResourceGroup(ImmutableArray<Resource> resources, ResourceGroupType type) =>
        resources
            .OfType<ResourceGroup>()
            .Single(x => x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(AzureResourceType.ResourceGroup, type.WorkloadName), StringComparison.Ordinal));
    
    private static UserAssignedIdentity GetManagedIdentity(ImmutableArray<Resource> resources, ResourceGroupType type) =>
        resources
            .OfType<UserAssignedIdentity>()
            .Single(x => x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(AzureResourceType.UserAssignedManagedIdentity, type.WorkloadName), StringComparison.Ordinal));
    
    private static RoleAssignment GetRoleAssignment(ImmutableArray<Resource> resources, ResourceGroupType type, string instance) =>
        resources
            .OfType<RoleAssignment>()
            .Single(x => x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(AzureResourceType.RoleAssignment, $"{type.WorkloadName}-{instance}"), StringComparison.Ordinal));
}
