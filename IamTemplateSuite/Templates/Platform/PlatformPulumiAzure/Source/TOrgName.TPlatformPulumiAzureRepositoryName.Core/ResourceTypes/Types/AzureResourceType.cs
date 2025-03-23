using Ardalis.SmartEnum;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;

public sealed class AzureResourceType : SmartEnum<AzureResourceType>, IResourceType
{
    public static readonly AzureResourceType AzureContainerRegistry = new("azure-container-registry", 1, "acr");
    public static readonly AzureResourceType ContainerApp = new("container-app", 2, "ca");
    public static readonly AzureResourceType ContainerAppsEnvironment = new("container-apps-environment", 3, "cae");
    public static readonly AzureResourceType KeyVault = new("key-vault", 4, "kv");
    public static readonly AzureResourceType ResourceGroup = new("resource-group", 5, "rg");
    public static readonly AzureResourceType RoleAssignment = new("role-assignment", 6, "ra");
    public static readonly AzureResourceType UserAssignedManagedIdentity = new("user-assigned-managed-identity", 7, "id");
    public static readonly AzureResourceType Secret = new("secret", 8, "secret");
    
    public string Abbreviation { get; }

    private AzureResourceType(string name, int value, string abbreviation) : base(name, value) => 
        Abbreviation = abbreviation;
}
