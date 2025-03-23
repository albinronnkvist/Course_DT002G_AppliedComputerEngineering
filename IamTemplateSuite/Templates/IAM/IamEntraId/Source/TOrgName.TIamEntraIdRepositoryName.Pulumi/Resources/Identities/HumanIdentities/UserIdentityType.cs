using Ardalis.SmartEnum;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.DirectoryRoles;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.HumanIdentities;

internal sealed class UserIdentityType : SmartEnum<UserIdentityType>
{
    public static readonly UserIdentityType AlbinRonnkvist = new("albin-ronnkvist", 1, 
        "Albin Rönnkvist", [DirectoryRoleType.GroupsAdministrator]);
    
    public static readonly UserIdentityType PiranAmedi = new("piran-amedi", 2, 
        "Roberto Piran Amedi", []);
    
    public string DisplayName { get; }
    public IReadOnlyCollection<DirectoryRoleType> DirectoryRoles { get; }

    private UserIdentityType(string name, int value, string displayName,
        IReadOnlyCollection<DirectoryRoleType> directoryRoles) : base(name, value)
    {
        DisplayName = displayName;
        DirectoryRoles = directoryRoles;
    }
}
