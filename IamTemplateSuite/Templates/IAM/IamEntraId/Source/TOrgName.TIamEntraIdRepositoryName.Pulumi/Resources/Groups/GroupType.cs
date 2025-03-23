using Ardalis.SmartEnum;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Groups;

internal sealed class GroupType : SmartEnum<GroupType>
{
    public static readonly GroupType Developers = new("developers", 1, "Developers");

    public string DisplayName { get; }

    private GroupType(string name, int value, string displayName) : base(name, value) => 
        DisplayName = displayName;
}
