using System.Collections.Immutable;
using System.Text.Json;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.Testing;
using Shouldly;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Test;

public sealed partial class MainStackTests
{
    // There is no API to mock config, and the actual configuration is taken from an environment variable PULUMI_CONFIG: https://github.com/pulumi/pulumi/issues/4472
    public MainStackTests() =>
        Environment.SetEnvironmentVariable("PULUMI_CONFIG", JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "azure-native:tenantId", "11111111-2222-3333-4444-e9dfca42d88a" },
            { "azure-native:subscriptionId", "33333333-4444-5555-6666-f026a4c7bc47" },
            { "azure-native:location", "swedencentral" }
        }));
    
    [Fact]
    public async Task SelfSubscriptionOwnerRoleAssignmentShouldExist()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        
        resources.OfType<RoleAssignment>()
            .Single(x => 
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(AzureResourceType.RoleAssignment, "TIamAzureRepositoryGitLabPath-spowner"), StringComparison.Ordinal))
            .ShouldNotBeNull();
    }
    
    private static async Task<ImmutableArray<Resource>> TestAsync() =>
        await Deployment.TestAsync<MainStack>(new Mocks(), new TestOptions { IsPreview = false });
}
