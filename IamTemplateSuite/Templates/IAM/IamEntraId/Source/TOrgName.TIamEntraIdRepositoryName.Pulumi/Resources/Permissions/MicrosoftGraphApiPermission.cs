using Ardalis.SmartEnum;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Permissions;

internal sealed class MicrosoftGraphApiPermission : SmartEnum<MicrosoftGraphApiPermission>
{
    public static readonly MicrosoftGraphApiPermission DirectoryReadAll = new("Directory.Read.All", 1, "7ab1d382-f21e-4acd-a863-ba3e13f7da61");
    public static readonly MicrosoftGraphApiPermission DirectoryReadWriteAll = new("Directory.ReadWrite.All", 2, "19dbc75e-c2e2-444c-a770-ec69d8559fc7");
    public static readonly MicrosoftGraphApiPermission RoleManagementReadWriteDirectory = new("RoleManagement.ReadWrite.Directory", 3, "9e3f62cf-ca93-4989-b6ce-bf83c28f9fe8");
    public static readonly MicrosoftGraphApiPermission ApplicationReadWriteAll = new("Application.ReadWrite.All", 4, "1bfefb4e-e0b5-418b-a88f-73c46d2cc8e9");
    public static readonly MicrosoftGraphApiPermission UserReadWriteAll = new("User.ReadWrite.All", 5, "741f803b-c850-494e-b5df-cde7c675a1ca");
    
    public string PermissionId { get; }

    private MicrosoftGraphApiPermission(string name, int value, string permissionId) : base(name, value) => PermissionId = permissionId;
}
