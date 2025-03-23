using TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options;
using Microsoft.Extensions.Options;

namespace TOrgName.TMicroserviceRepositoryName.Api.Endpoints;

internal static class ExamplesEndpointGroup
{
    internal static RouteGroupBuilder MapExamplesEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapGet("/", GetExamples);
        
        return builder;
    }
    
    private static async Task<IResult> GetExamples(IOptions<ExampleOptions> options)
    { 
        await Task.Delay(200);
        return TypedResults.Ok($"Example property: {options.Value.ExampleProperty}. Example secret: {options.Value.ExampleSecret}.");
    }
}
