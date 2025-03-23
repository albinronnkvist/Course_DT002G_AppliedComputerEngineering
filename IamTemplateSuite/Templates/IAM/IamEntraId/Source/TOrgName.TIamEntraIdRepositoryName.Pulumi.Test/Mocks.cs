using Pulumi.Testing;
using NSubstitute;
using Pulumi.AzureAD;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Test;

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
                
                if (string.Equals(args.Type, "azuread:index/user:User", StringComparison.Ordinal))
                {
                    outputs["objectId"] = Guid.NewGuid().ToString();
                }
                
                string applicationClientId = "168e4c33-0539-4387-925a-db077fada722";
                if (string.Equals(args.Type, "azuread:index/application:Application", StringComparison.Ordinal))
                {
                    outputs["clientId"] = applicationClientId;
                    outputs["objectId"] = Guid.NewGuid().ToString();
                }
                
                if (string.Equals(args.Type, "azuread:index/servicePrincipal:ServicePrincipal", StringComparison.Ordinal))
                {
                    outputs["clientId"] = applicationClientId;
                    outputs["objectId"] = Guid.NewGuid().ToString();
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
  
    public async Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args) =>  await _mock.NewResourceAsync(args);

    public async Task<object> CallAsync(MockCallArgs args) => await _mock.CallAsync(args);
}
