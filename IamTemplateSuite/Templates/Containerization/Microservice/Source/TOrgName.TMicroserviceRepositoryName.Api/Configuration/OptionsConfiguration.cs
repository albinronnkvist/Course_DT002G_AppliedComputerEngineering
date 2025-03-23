using FluentValidation;
using TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options;
using TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options.Helpers;
using TOrgName.TMicroserviceRepositoryName.Api.Validators;

namespace TOrgName.TMicroserviceRepositoryName.Api.Configuration;

internal static class OptionsConfiguration
{
    internal static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        services.ConfigureExampleOptions();
        services.ConfigureKeyVaultOptions();
        services.ConfigureManagedIdentityOptions();

        return services;
    }

    private static IServiceCollection ConfigureExampleOptions(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ExampleOptions>, ExampleOptionsValidator>();

        services.AddOptions<ExampleOptions>()
            .BindConfiguration(ExampleOptions.SectionName)
            .ValidateFluently()
            .ValidateOnStart();

        return services;
    }
    
    private static IServiceCollection ConfigureKeyVaultOptions(this IServiceCollection services)
    {
        services.AddScoped<IValidator<KeyVaultOptions>, KeyVaultOptionsValidator>();

        services.AddOptions<KeyVaultOptions>()
            .BindConfiguration(KeyVaultOptions.SectionName)
            .ValidateFluently()
            .ValidateOnStart();

        return services;
    }
    private static IServiceCollection ConfigureManagedIdentityOptions(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ManagedIdentityOptions>, ManagedIdentityOptionsValidator>();

        services.AddOptions<ManagedIdentityOptions>()
            .BindConfiguration(ManagedIdentityOptions.SectionName)
            .ValidateFluently()
            .ValidateOnStart();

        return services;
    }
}
