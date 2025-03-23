using System.Collections.Immutable;
using System.Text.Json;

namespace TOrgName.TContainerizationRepositoryName.Pulumi.StackReferences.TIamAzureRepositoryNameStackReference.Mappers;

internal static class ResourceGroupMapper
{
    private static readonly Dictionary<TOrgName.TIamAzureRepositoryName.Pulumi.Contract.Enums.ResourceGroupType, ResourceGroupType> Mappings = new()
    {
        [TIamAzureRepositoryName.Pulumi.Contract.Enums.ResourceGroupType.TContainerizationRepositoryName] = ResourceGroupType.InfrastructureContainerization,
        [TIamAzureRepositoryName.Pulumi.Contract.Enums.ResourceGroupType.TMicroserviceRepositoryName] = ResourceGroupType.ExampleMicroservice
    };
    
    internal static ImmutableDictionary<ResourceGroupType, ResourceGroup> Map(ImmutableDictionary<string, object> resourceGroups) =>
        resourceGroups.ToImmutableDictionary(
            kvp => MapResourceGroupType(ParseResourceGroupType(kvp.Key)),
            kvp => MapResourceGroup(ParseResourceGroup(kvp))
        );

    private static TOrgName.TIamAzureRepositoryName.Pulumi.Contract.Enums.ResourceGroupType ParseResourceGroupType(string typeString) =>
        Enum.Parse<TOrgName.TIamAzureRepositoryName.Pulumi.Contract.Enums.ResourceGroupType>(typeString);

    private static TOrgName.TIamAzureRepositoryName.Pulumi.Contract.OutputTypes.ResourceGroup ParseResourceGroup(KeyValuePair<string, object> kvp)
    {
        if (kvp.Value is not string json)
        {
            throw new InvalidOperationException($"Expected string value for ResourceGroupType key: {kvp.Key}");
        }

        return JsonSerializer.Deserialize<TOrgName.TIamAzureRepositoryName.Pulumi.Contract.OutputTypes.ResourceGroup>(json)
               ?? throw new InvalidOperationException($"Failed to deserialize ResourceGroupType: {kvp.Key}");
    }
    
    private static ResourceGroupType MapResourceGroupType(TOrgName.TIamAzureRepositoryName.Pulumi.Contract.Enums.ResourceGroupType contractResourceGroup) => 
        Mappings.TryGetValue(contractResourceGroup, out var resourceGroup) 
            ? resourceGroup 
            : throw new KeyNotFoundException($"No contract mapping found for ResourceGroupType '{contractResourceGroup.ToString()}'.");
    
    private static ResourceGroup MapResourceGroup(TOrgName.TIamAzureRepositoryName.Pulumi.Contract.OutputTypes.ResourceGroup resourceGroup) => 
        new(resourceGroup.ResourceGroupName, resourceGroup.ManagedIdentityId, resourceGroup.ManagedIdentityClientId, resourceGroup.ManagedIdentityPrincipalId);
}
