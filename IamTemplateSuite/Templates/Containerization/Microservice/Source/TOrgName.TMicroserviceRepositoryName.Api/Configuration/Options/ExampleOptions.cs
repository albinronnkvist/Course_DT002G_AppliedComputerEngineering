namespace TOrgName.TMicroserviceRepositoryName.Api.Configuration.Options;

internal sealed record ExampleOptions
{
    internal const string SectionName = "Example";

    public string ExampleProperty { get; init; } = null!;
    public string ExampleSecret { get; init; } = null!;
}
