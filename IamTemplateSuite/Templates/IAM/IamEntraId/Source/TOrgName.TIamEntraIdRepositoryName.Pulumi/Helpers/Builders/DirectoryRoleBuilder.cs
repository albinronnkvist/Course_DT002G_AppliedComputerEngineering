using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.DirectoryRoles;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

internal class DirectoryRoleBuilder(string resourceName, 
    DirectoryRoleType roleType, CustomResourceOptions? options = null)
{
    public DirectoryRole Build() =>
        new(resourceName, new()
        {
            TemplateId = roleType.TemplateId
        }, options);
}
