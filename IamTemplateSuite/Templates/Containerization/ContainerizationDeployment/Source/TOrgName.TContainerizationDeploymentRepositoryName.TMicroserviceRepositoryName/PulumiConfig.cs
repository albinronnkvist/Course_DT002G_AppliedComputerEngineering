using Pulumi;

namespace TOrgName.TContainerizationDeploymentRepositoryName.TMicroserviceRepositoryName;

internal sealed class PulumiConfig
{
    private readonly Config _defaultConfig = new();
    private readonly Config _azureNativeConfig = new("azure-native");
    
    public string TenantId => _azureNativeConfig.Require("tenantId");
    public string SubscriptionId => _azureNativeConfig.Require("subscriptionId");
    public string ClientId => _azureNativeConfig.Require("clientId");
    
    public string ImageTagApi => _defaultConfig.Require("imageTagApi");
    public string DotnetEnvironment => _defaultConfig.Require("dotnetEnvironment");
}
