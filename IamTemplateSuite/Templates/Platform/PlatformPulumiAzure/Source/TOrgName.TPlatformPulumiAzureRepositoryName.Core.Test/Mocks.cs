using NSubstitute;
using Pulumi.Testing;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.Test;

internal sealed class Mocks : IMocks
{
    private readonly IMocks _mock;
    
    public Mocks()
    {
        _mock = Substitute.For<IMocks>();
        
        _mock.NewResourceAsync(Arg.Any<MockResourceArgs>())
            .Returns(callInfo =>
            {
                MockResourceArgs? args = callInfo.Arg<MockResourceArgs>();
                var outputs = new Dictionary<string, object>(args.Inputs);
                
                if (string.Equals(args.Type, "azure-native:keyvault:Secret", StringComparison.Ordinal))
                {
                    outputs["properties"] = new Dictionary<string, object>
                    {
                        ["secretUri"] = "https://microservice-dev.vault.azure.net/"
                    };
                }

                return Task.FromResult<(string?, object)>((args.Id ?? string.Empty, outputs));
            });
        
        _mock.CallAsync(Arg.Any<MockCallArgs>())
            .Returns(callInfo =>
            {
                MockCallArgs? args = callInfo.Arg<MockCallArgs>();
                return Task.FromResult(args.Args);
            });

    }
  
    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args) =>  _mock.NewResourceAsync(args);

    public Task<object> CallAsync(MockCallArgs args) => _mock.CallAsync(args);
}
