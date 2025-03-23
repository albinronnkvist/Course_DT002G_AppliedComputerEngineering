using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;

public sealed class SecretBuilder(string resourceName,
    string secretName, Input<string> secretValue, string resourceGroupName, Input<string> vaultName, CustomResourceOptions? options = null)
{
    public Secret Build() => new(resourceName, new SecretArgs
    {
        SecretName = secretName,
        Properties = new SecretPropertiesArgs
        {
            Value = secretValue
        },
        ResourceGroupName = resourceGroupName,
        VaultName = vaultName
    }, options);
}
