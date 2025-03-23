using Ardalis.SmartEnum;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Permissions;

internal sealed class ApiPermissionType : SmartEnum<ApiPermissionType>
{
    public static readonly ApiPermissionType Application = new("Role", 1);
    public static readonly ApiPermissionType Delegated = new("Scope", 2);

    private ApiPermissionType(string name, int value) : base(name, value) {}
}
