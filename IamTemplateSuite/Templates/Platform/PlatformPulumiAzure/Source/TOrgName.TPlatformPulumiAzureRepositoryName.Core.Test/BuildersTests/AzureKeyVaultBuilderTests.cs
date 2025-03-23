using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.Testing;
using Shouldly;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.Test.BuildersTests;

public class AzureKeyVaultBuilderTests
{
    [Fact]
    public async Task Test()
    {
        var resources = await Deployment.TestAsync<TestStack>(new Mocks(), new TestOptions { IsPreview = false });
        var resource = resources.OfType<Vault>().Single();
        var properties = await resource.Properties.GetValueAsync();
        
        resource.GetResourceName().ShouldBe(TestConstants.ResourceName);
        properties.Sku.Name.ShouldBe(SkuName.Standard.ToString());
        properties.Sku.Family.ShouldBe(SkuFamily.A.ToString());
        properties.EnableRbacAuthorization.ShouldBe(true);
        properties.TenantId.ShouldBe(TestConstants.TenantId);
    }
    
    private static class TestConstants
    {
        public const string ResourceName = "key-vault-resource";
        public const string VaultName = "kv-microservice-dev";
        public const string ResourceGroupName = "rg-name";
        public const string TenantId = "0795b0ee-8c5a-437d-a2eb-f89ab7e3157d";
    }
    
    private sealed class TestStack : Stack
    {
        public TestStack() => _ = new AzureKeyVaultBuilder(TestConstants.ResourceName, 
                TestConstants.VaultName, TestConstants.ResourceGroupName, TestConstants.TenantId)
            .Build();
    }
}
