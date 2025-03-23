using Ardalis.SmartEnum;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Domains;

internal sealed class DomainType : SmartEnum<DomainType>
{
    public static readonly DomainType Default = new DomainType("TDomainName", 1);

    private DomainType(string name, int value) 
        : base(name, value) {}
}
