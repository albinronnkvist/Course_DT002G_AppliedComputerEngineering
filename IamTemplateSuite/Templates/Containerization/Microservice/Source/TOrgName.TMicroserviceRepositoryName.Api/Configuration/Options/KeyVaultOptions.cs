namespace TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options;

internal sealed class KeyVaultOptions
{
    internal const string SectionName = "KeyVault";

    public string VaultUri { get; init; } = null!;
}
