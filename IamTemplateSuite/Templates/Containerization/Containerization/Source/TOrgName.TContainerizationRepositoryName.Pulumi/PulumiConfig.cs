using Pulumi;

namespace TOrgName.TContainerizationRepositoryName.Pulumi;

internal sealed class PulumiConfig
{
    private readonly Config _azureNativeConfig = new("azure-native");
    public string ServicePrincipalClientId => _azureNativeConfig.Require("clientId");
    public string SubscriptionId => _azureNativeConfig.Require("subscriptionId");
}
