using FluentValidation;
using TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options;

namespace TOrgName.TMicroserviceRepositoryName.Api.Validators;

internal sealed class ExampleOptionsValidator : AbstractValidator<ExampleOptions>
{
    public ExampleOptionsValidator()
    {
        RuleFor(x => x.ExampleProperty).NotEmpty();
        RuleFor(x => x.ExampleSecret).NotEmpty();
    }
}
