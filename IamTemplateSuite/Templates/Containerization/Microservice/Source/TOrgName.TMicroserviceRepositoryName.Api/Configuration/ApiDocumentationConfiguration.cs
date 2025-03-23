using Scalar.AspNetCore;

namespace TOrgName.TMicroserviceRepositoryName.Api.Configuration;

internal static class ApiDocumentationConfiguration
{
    internal static IServiceCollection ConfigureApiDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi();

        return services;
    }
    
    internal static WebApplication ConfigureApiDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options
                    .WithTitle("TMicroserviceRepositoryName Api")
                    .WithTheme(ScalarTheme.Mars)
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }

        return app;
    }
}
