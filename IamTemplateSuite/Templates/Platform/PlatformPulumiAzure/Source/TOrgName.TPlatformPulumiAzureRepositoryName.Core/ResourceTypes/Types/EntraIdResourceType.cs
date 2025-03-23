using Ardalis.SmartEnum;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;

public sealed class EntraIdResourceType : SmartEnum<EntraIdResourceType>, IResourceType
{
    public static readonly EntraIdResourceType User = new("user", 1, "user");
    public static readonly EntraIdResourceType Group = new("group", 2, "group");
    public static readonly EntraIdResourceType DirectoryRole = new("directory-role", 3, "dr");
    public static readonly EntraIdResourceType DirectoryRoleAssignment = new("directory-role-assignment", 4, "dra");
    public static readonly EntraIdResourceType Application = new("application", 5, "app");
    public static readonly EntraIdResourceType ServicePrincipal = new("service-principal", 6, "sp");
    public static readonly EntraIdResourceType ServicePrincipalDelegatedPermissionGrant = new("service-principal-delegated-permission-grant", 7, "spdpg");
    public static readonly EntraIdResourceType FederatedIdentityCredential = new("federated-identity-credential", 8, "fic");
    public static readonly EntraIdResourceType AppRoleAssignment = new("app-role-assignment", 9, "ar");
    public string Abbreviation { get; }

    private EntraIdResourceType(string name, int value, string abbreviation) : base(name, value) => 
        Abbreviation = abbreviation;
}
