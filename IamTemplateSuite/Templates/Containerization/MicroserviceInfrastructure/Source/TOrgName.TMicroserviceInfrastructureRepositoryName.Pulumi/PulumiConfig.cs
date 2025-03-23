using Pulumi;

namespace TOrgName.TMicroserviceInfrastructureRepositoryName.Pulumi;

internal sealed class PulumiConfig
{
    private readonly Config _defaultConfig = new();
    private readonly Config _azureNativeConfig = new("azure-native");
    
    public string TenantId => _azureNativeConfig.Require("tenantId");
    public string SubscriptionId => _azureNativeConfig.Require("subscriptionId");
    
    // Secrets
    public Output<string> ExampleSecret => _defaultConfig.RequireSecret("exampleSecret");
}
