using System.Collections.Immutable;
using NSubstitute;
using Pulumi.Testing;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Test;

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
                
                if (args.Type == "pulumi:pulumi:StackReference")
                {
                    var outputs = new Dictionary<string, object>
                    {
                        [TIamEntraIdRepositoryName.Pulumi.Contract.StackOutputs.ApplicationIdentities] = 
                            StackOutputMocksHelper.GetTIamEntraIdRepositoryNameStackOutput(TIamEntraIdRepositoryName.Pulumi.Contract.StackOutputs.ApplicationIdentities)
                    }.ToImmutableDictionary();

                    var props = new Dictionary<string, object>
                    {
                        { "name", args.Inputs["name"] },
                        { "outputs", outputs },
                        { "secretOutputNames", ImmutableArray<string>.Empty }
                    };
                    
                    return Task.FromResult<(string?, object)>((args.Id ?? $"{args.Name}_id", props));
                }
                else
                {
                    var outputs = new Dictionary<string, object>(args.Inputs);
                
                    return Task.FromResult<(string?, object)>((args.Id ?? $"{args.Name}_id", outputs));
                }
            });
        
        _mock.CallAsync(Arg.Any<MockCallArgs>())
            .Returns(callInfo =>
            {
                MockCallArgs? args = callInfo.Arg<MockCallArgs>();
                return Task.FromResult(args.Args);
            });

    }
  
    public async Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args) => await _mock.NewResourceAsync(args);

    public async Task<object> CallAsync(MockCallArgs args) => await _mock.CallAsync(args);
}
