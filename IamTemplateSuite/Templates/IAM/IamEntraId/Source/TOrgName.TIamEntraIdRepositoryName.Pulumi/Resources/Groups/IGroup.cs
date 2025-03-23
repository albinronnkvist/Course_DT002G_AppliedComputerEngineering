using Pulumi;
using Pulumi.AzureAD;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Groups;

internal interface IGroup
{
    public Output<Group> Group { get; }
}
