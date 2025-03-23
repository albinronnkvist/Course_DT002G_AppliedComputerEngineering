using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.Testing;
using Shouldly;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.Test.BuildersTests;

public class SecretBuilderTests
{
    [Fact]
    public async Task Test()
    {
        var resources = await Deployment.TestAsync<TestStack>(new Mocks(), new TestOptions { IsPreview = false });
        var resource = resources.OfType<Secret>().Single();
        
        resource.GetResourceName().ShouldBe(TestConstants.ResourceName);
        (await resource.Properties.GetValueAsync()).SecretUri.ShouldBe(TestConstants.MockedKeyVaultUri);
    }
    
    private static class TestConstants
    {
        public const string ResourceName = "secret-resource";
        public const string SecretName = "super-secret";
        public const string SecretValue = "secret-value";
        public const string ResourceGroupName = "rg-name";
        public const string VaultName = "my-vault";
        public const string MockedKeyVaultUri = "https://microservice-dev.vault.azure.net/";
    }
    
    private sealed class TestStack : Stack
    {
        public TestStack() => _ = new SecretBuilder(TestConstants.ResourceName, 
                TestConstants.SecretName, TestConstants.SecretValue, 
                TestConstants.ResourceGroupName, TestConstants.VaultName, new CustomResourceOptions())
            .Build();
    }
}
