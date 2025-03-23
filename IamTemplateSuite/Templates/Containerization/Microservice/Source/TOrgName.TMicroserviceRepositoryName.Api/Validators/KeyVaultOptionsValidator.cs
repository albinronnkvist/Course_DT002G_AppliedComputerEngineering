using FluentValidation;
using TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options;

namespace TOrgName.TMicroserviceRepositoryName.Api.Validators;

internal sealed class KeyVaultOptionsValidator : AbstractValidator<KeyVaultOptions>
{
    public KeyVaultOptionsValidator() =>
        RuleFor(x => x.VaultUri)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute));
}
