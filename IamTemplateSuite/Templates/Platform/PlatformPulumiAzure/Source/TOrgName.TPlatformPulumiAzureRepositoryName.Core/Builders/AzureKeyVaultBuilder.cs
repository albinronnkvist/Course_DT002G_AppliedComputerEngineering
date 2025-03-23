using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;

public sealed class AzureKeyVaultBuilder(string resourceName,
    string vaultName, string resourceGroupName, string tenantId, CustomResourceOptions? options = null)
{
    public Vault Build() =>
        new(resourceName, new VaultArgs
        {
            VaultName = vaultName,
            ResourceGroupName = resourceGroupName,
            Properties = new VaultPropertiesArgs
            {
                EnableRbacAuthorization = true,
                Sku = new SkuArgs
                {
                    Family = SkuFamily.A,
                    Name = SkuName.Standard
                },
                TenantId = tenantId
            }
        }, options);
}
