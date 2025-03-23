using System.Collections.Immutable;
using System.Text.Json;
using Pulumi;
using Pulumi.Testing;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Test;

public sealed partial class MainStackTests
{
    // There is no API to mock config, and the actual configuration is taken from an environment variable PULUMI_CONFIG: https://github.com/pulumi/pulumi/issues/4472
    public MainStackTests() =>
        Environment.SetEnvironmentVariable("PULUMI_CONFIG", JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "azuread:tenantId", "11111111-2222-3333-a6a4-e9dfca42d88a" },
            { "project:userInitialPassword", "#super-secure-password123!" }
        }));

    private static async Task<ImmutableArray<Resource>> TestAsync() => 
       await Deployment.TestAsync<MainStack>(new Mocks(), new TestOptions { IsPreview = false });
}
