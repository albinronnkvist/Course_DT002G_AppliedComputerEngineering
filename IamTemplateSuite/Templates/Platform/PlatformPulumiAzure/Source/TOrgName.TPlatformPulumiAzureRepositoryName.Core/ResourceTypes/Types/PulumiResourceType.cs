using Ardalis.SmartEnum;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;

public sealed class PulumiResourceType : SmartEnum<PulumiResourceType>, IResourceType
{
    public static readonly PulumiResourceType Command = new("command", 1, "cmd");
    public static readonly PulumiResourceType ComponentResource = new("component-resource", 2, "cr");
    
    public string Abbreviation { get; }

    private PulumiResourceType(string name, int value, string abbreviation) : base(name, value) => 
        Abbreviation = abbreviation;
}
