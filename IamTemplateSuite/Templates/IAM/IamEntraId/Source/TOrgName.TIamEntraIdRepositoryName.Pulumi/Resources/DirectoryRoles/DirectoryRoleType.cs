using Ardalis.SmartEnum;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.DirectoryRoles;

internal sealed class DirectoryRoleType : SmartEnum<DirectoryRoleType>
{
    public static readonly DirectoryRoleType GlobalAdministrator = new("global-administrator", 1, "62e90394-69f5-4237-9190-012177145e10", "Global Administrator");
    public static readonly DirectoryRoleType GroupsAdministrator = new("groups-administrator", 2, "fdd7a751-b60b-444a-984c-02652fe8fa1c", "Groups Administrator");
    public static readonly DirectoryRoleType CloudApplicationAdministrator = new("cloud-application-administrator", 3, "158c047a-c907-4556-b7ef-446551a6b5f7", "Cloud Application Administrator ");
    
    public string TemplateId { get; }
    public string DisplayName { get; }

    private DirectoryRoleType(string name, int value, string templateId, string displayName) : base(name, value)
    {
        TemplateId = templateId;
        DisplayName = displayName;
    } 
}
