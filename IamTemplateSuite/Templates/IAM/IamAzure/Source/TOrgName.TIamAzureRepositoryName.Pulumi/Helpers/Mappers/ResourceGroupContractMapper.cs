using TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceGroups;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Mappers;

internal static class ResourceGroupContractMapper
{
    private static readonly Dictionary<ResourceGroupType, Contract.Enums.ResourceGroupType> Mappings = new()
    {
        [ResourceGroupType.TContainerizationRepositoryName] = Contract.Enums.ResourceGroupType.TContainerizationRepositoryName,
        [ResourceGroupType.TMicroserviceRepositoryName] = Contract.Enums.ResourceGroupType.TMicroserviceRepositoryName

    };

    public static Contract.Enums.ResourceGroupType Map(ResourceGroupType resourceGroup) => Mappings.TryGetValue(resourceGroup, out Contract.Enums.ResourceGroupType contractType) 
        ? contractType 
        : throw new KeyNotFoundException($"No contract mapping found for ResourceGroupType '{resourceGroup.Name}'.");
}
