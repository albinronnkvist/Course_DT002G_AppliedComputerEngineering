using Ardalis.SmartEnum;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceProviders;

internal sealed class ResourceProviderType : SmartEnum<ResourceProviderType>
{
    public static readonly ResourceProviderType MicrosoftApp = new("microsoft-app", 1, "Microsoft.App");
    public static readonly ResourceProviderType MicrosoftKeyVault = new("microsoft-key-vault", 2, "Microsoft.KeyVault");
    public static readonly ResourceProviderType MicrosoftContainerRegistry = new("microsoft-container-registry", 3, "Microsoft.ContainerRegistry");
    public static readonly ResourceProviderType MicrosoftManagedIdentity = new("microsoft-managed-identity", 4, "Microsoft.ManagedIdentity");

    
    public string Namespace { get; }

    private ResourceProviderType(string name, int value, string @namespace) : base(name, value) => Namespace = @namespace;
}
