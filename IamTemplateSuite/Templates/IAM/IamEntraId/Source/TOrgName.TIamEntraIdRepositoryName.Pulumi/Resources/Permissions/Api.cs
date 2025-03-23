using Ardalis.SmartEnum;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Permissions;

internal sealed class Api : SmartEnum<Api>
{
    public static readonly Api MicrosoftGraph = new("microsoft-graph", 1, "00000003-0000-0000-c000-000000000000");
    
    public string ResourceAppId { get; }

    private Api(string name, int value, string resourceAppId) : base(name, value) => ResourceAppId = resourceAppId;
}
