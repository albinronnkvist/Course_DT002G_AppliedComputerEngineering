using System.Collections.Immutable;
using TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceProviders;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.Command.Local;
using Shouldly;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Test;

public sealed partial class MainStackTests
{
    [Fact]
    public async Task ResourceProvidersComponentResourceShouldExist()
    {
        ImmutableArray<Resource> resources= await TestAsync();
        
        resources.OfType<ComponentResource>()
            .Single(x => 
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "resource-providers"), StringComparison.Ordinal))
            .ShouldNotBeNull();
    }
    
    [Fact]
    public async Task ShouldCreateExpectedResourceProviderComponentResources()
    {
        ImmutableArray<Resource> resources = await TestAsync();
    
        var componentResourcesNames = resources.OfType<ComponentResource>()
            .Select(resourceProvider => resourceProvider.GetResourceName())
            .ToList();

        var resourceProviderComponentResourcesNames = ResourceProviderType.List
            .Select(resourceProvider =>
                ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, resourceProvider.Name))
            .ToList();
        
        resourceProviderComponentResourcesNames.ShouldBeSubsetOf(componentResourcesNames);
    }

    [Fact]
    public async Task ShouldCreateExpectedResourceProviderCommands()
    {
        ImmutableArray<Resource> resources = await TestAsync();
    
        var resourceProviderCommandsResourceNames = resources.OfType<Command>()
            .Select(resourceProvider => resourceProvider.GetResourceName())
            .ToList();
        
        resourceProviderCommandsResourceNames.ShouldBe(ResourceProviderType.List
                .Select(resourceProvider => ResourceNamingConvention.GetResourceName(PulumiResourceType.Command, resourceProvider.Name))
                .ToList(),
            true
        );
    }
}
