namespace TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options;

internal sealed class ManagedIdentityOptions
{
    internal const string SectionName = "ManagedIdentity";

    public string ClientId { get; init; } = null!;
}
