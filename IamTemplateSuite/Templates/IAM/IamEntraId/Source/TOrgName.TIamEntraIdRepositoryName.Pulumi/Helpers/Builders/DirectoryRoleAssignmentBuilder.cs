using Pulumi;
using Pulumi.AzureAD;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

internal class DirectoryRoleAssignmentBuilder(string resourceName, 
    Input<string> roleTemplateId, Input<string> principalObjectId, CustomResourceOptions? options = null)
{
    public DirectoryRoleAssignment Build() =>
        new(resourceName, new()
        {
            RoleId = roleTemplateId,
            PrincipalObjectId = principalObjectId
        }, options);
}
