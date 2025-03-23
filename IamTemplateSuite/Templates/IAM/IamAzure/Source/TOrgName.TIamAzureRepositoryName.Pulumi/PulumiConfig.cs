using Pulumi;

namespace TOrgName.TIamAzureRepositoryName.Pulumi;

internal sealed class PulumiConfig
{
    private readonly Config _azureNativeConfig = new("azure-native");
    
    public string TenantId => _azureNativeConfig.Require("tenantId");
    public string SubscriptionId => _azureNativeConfig.Require("subscriptionId");
}
