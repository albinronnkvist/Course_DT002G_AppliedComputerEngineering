using System.Collections.Immutable;
using TOrgName.TPlatformPulumiRepositoryName.Core.Helpers;
using Pulumi;
using Pulumi.Testing;
using Shouldly;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Test;

public sealed partial class MainStackTests
{
    [Fact]
    public async Task ApplicationIdentitiesStackOutputShouldReturnApplicationIdentities()
    {
        (_, IDictionary<string, object?> outputs) = await TestStackReferenceOutputsAsync();
        
        var applicationIdentities = outputs[TIamEntraIdRepositoryName.Pulumi.Contract.StackOutputs.ApplicationIdentities] as StackReferenceOutputDetails;
        
        applicationIdentities.ShouldNotBeNull();
        applicationIdentities.Value.ShouldBe(StackOutputMocksHelper.GetTIamEntraIdRepositoryNameStackOutput(TIamEntraIdRepositoryName.Pulumi.Contract.StackOutputs.ApplicationIdentities));
        applicationIdentities.SecretValue.ShouldBeNull();
    }
    
    private static async Task<(ImmutableArray<Resource>, IDictionary<string, object?>)> TestStackReferenceOutputsAsync() =>
        await Deployment.TestAsync(
            new Mocks(), new TestOptions { IsPreview = false },
            async () =>
            {
                StackReference stackReference = StackReferenceHelper.GetStackReference(TIamEntraIdRepositoryName.Pulumi.Contract.StackDetails.Organization, 
                    TIamEntraIdRepositoryName.Pulumi.Contract.StackDetails.Project, 
                    TIamEntraIdRepositoryName.Pulumi.Contract.StackNames.Main);

                return new Dictionary<string, object?>
                {
                    [TIamEntraIdRepositoryName.Pulumi.Contract.StackOutputs.ApplicationIdentities] = 
                        await stackReference.GetOutputDetailsAsync(TIamEntraIdRepositoryName.Pulumi.Contract.StackOutputs.ApplicationIdentities)
                };
            });
}
