using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using TOrgName.TMicroserviceRepositoryName.Api.Endpoints;

namespace TOrgName.TMicroserviceRepositoryName.Api.Configuration;

internal static class EndpointRoutingConfiguration
{
    private const string RoutePrefix = "api/v{version:apiVersion}";
    
    internal static WebApplication MapEndpoints(this WebApplication app)
    {
        var versionOneSet = app.NewApiVersionSet()
            .HasApiVersion(1.0)
            .ReportApiVersions()
            .Build();

        MapExamplesEndpointGroup(app, versionOneSet);

        return app;
    }


    private static WebApplication MapExamplesEndpointGroup(this WebApplication app, ApiVersionSet versionOneSet)
    {
        app.MapGroup($"{RoutePrefix}/examples")
        .MapExamplesEndpoints()
            .WithApiVersionSet(versionOneSet)
            .MapToApiVersion(1.0);
        
        return app;
    }
}
