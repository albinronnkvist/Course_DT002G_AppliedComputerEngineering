using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.Testing;
using Shouldly;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.RoleAssignments;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.Test.BuildersTests;

public class RoleAssignmentBuilderTests
{
    [Fact]
    public async Task Test()
    {
        var resources = await Deployment.TestAsync<TestStack>(new Mocks(), new TestOptions { IsPreview = false });
        var resource = resources.OfType<RoleAssignment>().Single();

        resource.GetResourceName().ShouldBe(TestConstants.ResourceName);
        (await resource.PrincipalId.GetValueAsync()).ShouldBe(TestConstants.PrincipalId);
        (await resource.PrincipalType.GetValueAsync()).ShouldBe(TestConstants.PrincipalType.ToString());
        (await resource.RoleDefinitionId.GetValueAsync()).ShouldBe(TestConstants.AzureRoleType.RoleDefinitionId);
        (await resource.Scope.GetValueAsync()).ShouldBe(await TestConstants.Scope.GetValueAsync());
    }
    
    private static class TestConstants
    {
        public const string ResourceName = "role-assignment-resource";
        public const string PrincipalId = "kv-microservice-dev";
        public static readonly PrincipalType PrincipalType = PrincipalType.ServicePrincipal;
        public static readonly AzureRoleType AzureRoleType = AzureRoleType.Owner;
        public static readonly Output<string> Scope = Output.Create("/subscriptions/87685d7c-367f-448d-8d0f-5df6c5882808/resourcegroups/rg-microservice-dev/providers/Microsoft.ManagedIdentity/userAssignedIdentities/id-microservice-dev");
    }
    
    private sealed class TestStack : Stack
    {
        public TestStack() => _ = new RoleAssignmentBuilder(TestConstants.ResourceName, 
                TestConstants.PrincipalId, TestConstants.PrincipalType, TestConstants.AzureRoleType, TestConstants.Scope)
            .Build();
    }
}
