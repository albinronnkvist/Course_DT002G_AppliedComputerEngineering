using FluentValidation;
using Microsoft.Extensions.Options;

namespace TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options.Helpers;

// TODO: move to external project
internal sealed class OptionsFluentValidator<TOptions>(IServiceProvider serviceProvider, string? name)
    : IValidateOptions<TOptions> where TOptions : class
{
    public ValidateOptionsResult Validate(string? sectionName, TOptions options)
    {
        if (!string.IsNullOrWhiteSpace(name) && name != sectionName)
        {
            return ValidateOptionsResult.Skip;
        }
        
        ArgumentNullException.ThrowIfNull(options);
        
        using var scope = serviceProvider.CreateScope();
        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();
        
        var result = validator.Validate(options);
        if (result.IsValid)
        {
            return ValidateOptionsResult.Success;
        }
        
        return ValidateOptionsResult.Fail(result.Errors.Select(x => 
            $"{options.GetType().Name}: {x.PropertyName} - {x.ErrorMessage}"));
    }
}
