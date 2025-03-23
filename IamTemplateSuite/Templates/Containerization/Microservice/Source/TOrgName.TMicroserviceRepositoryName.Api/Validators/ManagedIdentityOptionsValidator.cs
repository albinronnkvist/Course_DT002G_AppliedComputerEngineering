using FluentValidation;
using TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options;

namespace TOrgName.TMicroserviceRepositoryName.Api.Validators;

internal sealed class ManagedIdentityOptionsValidator : AbstractValidator<ManagedIdentityOptions>
{
    public ManagedIdentityOptionsValidator() =>
        RuleFor(x => x.ClientId)
            .NotEmpty();
}
