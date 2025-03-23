using Pulumi;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi;

internal sealed class PulumiConfig
{
    private readonly Config _defaultConfig = new();
    private readonly Config _azureAdConfig = new("azuread");
    
    public string TenantId => _azureAdConfig.Require("tenantId");

    public Output<string> UserInitialPassword => _defaultConfig.RequireSecret("userInitialPassword");
}
