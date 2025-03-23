using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Groups;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

internal class GroupBuilder(string resourceName, 
    GroupType groupType, CustomResourceOptions? options = null)
{
    private readonly InputList<string> _owners = new();
    private readonly InputList<string> _members = new();
    private string _description = string.Empty;
    private bool _isAssignableToRole;

    public GroupBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public GroupBuilder WithOwners(IReadOnlyCollection<User> users)
    {
        foreach (User user in users)
        {
            _owners.Add(user.ObjectId);
        }
        return this;
    }
    
    public GroupBuilder WithMembers(IReadOnlyCollection<User> users)
    {
        foreach (User user in users)
        {
            _members.Add(user.ObjectId);
        }
        return this;
    }
    
    public GroupBuilder WithAssignableToRole(bool enabled) 
    {
        _isAssignableToRole = enabled;
        return this;
    }

    public Group Build() =>
        new(resourceName, new()
        {
            DisplayName = groupType.DisplayName,
            Description = _description,
            SecurityEnabled = true,
            AssignableToRole = _isAssignableToRole,
            Owners = _owners,
            Members = _members
        }, options);
}
