using Microsoft.Extensions.Options;

namespace TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options.Helpers;

// TODO: move to external project
internal static class OptionsBuilderExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(
        this OptionsBuilder<TOptions> builder) where TOptions : class
    {
        builder.Services.AddSingleton<IValidateOptions<TOptions>>(serviceProvider =>
            new OptionsFluentValidator<TOptions>(serviceProvider, builder.Name));
        
        return builder;
    }
}
