using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.Testing;
using Shouldly;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;
using Deployment = Pulumi.Deployment;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.Test.BuildersTests;

public class ResourceGroupBuilderTests
{
    [Fact]
    public async Task Test()
    {
        var resources = await Deployment.TestAsync<TestStack>(new Mocks(), new TestOptions { IsPreview = false });
        var resource = resources.OfType<ResourceGroup>().Single();
        
        resource.GetResourceName().ShouldBe(TestConstants.ResourceName);
    }
    
    private static class TestConstants
    {
        public const string ResourceName = "rg-TOrgName-dev";
        public const string ResourceGroupName = "rg-TOrgName";
    }

    private sealed class TestStack : Stack
    {
        public TestStack() => _ = new ResourceGroupBuilder(TestConstants.ResourceName, TestConstants.ResourceGroupName).Build();
    }
}

