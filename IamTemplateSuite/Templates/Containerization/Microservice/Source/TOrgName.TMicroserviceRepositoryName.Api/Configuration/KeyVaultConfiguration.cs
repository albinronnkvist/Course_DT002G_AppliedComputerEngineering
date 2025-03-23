using Azure.Identity;
using TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options;

namespace TOrgName.TMicroserviceRepositoryName.Api.Configuration;

internal static class KeyVaultConfiguration
{
    internal static IConfigurationManager ConfigureKeyVault(this IConfigurationManager configurations)
    {
        var vaultUri = configurations[$"{KeyVaultOptions.SectionName}:{nameof(KeyVaultOptions.VaultUri)}"];
        ArgumentNullException.ThrowIfNull(vaultUri);
        
        var managedIdentityClientId = configurations[$"{ManagedIdentityOptions.SectionName}:{nameof(ManagedIdentityOptions.ClientId)}"];
        ArgumentNullException.ThrowIfNull(managedIdentityClientId);
        
        configurations.AddAzureKeyVault(new Uri(vaultUri), new DefaultAzureCredential(new DefaultAzureCredentialOptions
       {
           ManagedIdentityClientId = managedIdentityClientId
       }));
       
        return configurations;
    }
}
