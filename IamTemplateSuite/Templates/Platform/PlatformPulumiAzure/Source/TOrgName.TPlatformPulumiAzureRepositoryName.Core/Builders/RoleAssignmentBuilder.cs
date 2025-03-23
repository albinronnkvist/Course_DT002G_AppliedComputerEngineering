using Pulumi;
using Pulumi.AzureNative.Authorization;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.RoleAssignments;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;

public sealed class RoleAssignmentBuilder(string resourceName,
    string principalId, PrincipalType principalType, AzureRoleType azureRoleType, Output<string> scope, CustomResourceOptions? options = null)
{
    public RoleAssignment Build() =>
        new(resourceName, new RoleAssignmentArgs
        {
            RoleDefinitionId = azureRoleType.RoleDefinitionId,
            PrincipalId = principalId,
            PrincipalType = principalType,
            Scope = scope
        }, options);
}
