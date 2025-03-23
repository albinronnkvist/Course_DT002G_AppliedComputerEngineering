using System.Collections.Immutable;
using System.Text.Json;
using Pulumi;
using TOrgName.TIamAzureRepositoryName.Pulumi.Contract;
using TOrgName.TIamAzureRepositoryName.Pulumi.Contract.Enums;

namespace TOrgName.TMicroserviceInfrastructureRepositoryName.Pulumi.StackReferences.TIamAzureRepositoryNameStackReference.Mappers;

internal static class TIamAzureRepositoryNameStackOutputMapper
{
    internal static async Task<ResourceGroup> GetResourceGroupAsync(StackReference stackReference)
    {
        var outputDetails = await stackReference
            .GetOutputDetailsAsync(StackOutputs.ResourceGroups);
        
        if (outputDetails.Value is not ImmutableDictionary<string, object> resourceGroups)
        {
            throw new InvalidOperationException($"Stack reference output '{StackOutputs.ResourceGroups}' was not a valid type.");
        }
        
        if (!resourceGroups.TryGetValue(
                ResourceGroupType.TMicroserviceRepositoryName.ToString(),
                out var exampleMicroserviceResourceGroup))
        {
            throw new KeyNotFoundException($"ResourceGroupType '{ResourceGroupType.TMicroserviceRepositoryName}' key not found.");
        }

        return MapResourceGroup(ParseResourceGroup(exampleMicroserviceResourceGroup));
    }
    
    private static ResourceGroup MapResourceGroup(TOrgName.TIamAzureRepositoryName.Pulumi.Contract.OutputTypes.ResourceGroup resourceGroup) => 
        new(resourceGroup.ResourceGroupName, resourceGroup.ManagedIdentityId, resourceGroup.ManagedIdentityClientId, resourceGroup.ManagedIdentityPrincipalId);
    
    private static TOrgName.TIamAzureRepositoryName.Pulumi.Contract.OutputTypes.ResourceGroup ParseResourceGroup(object resourceGroup)
    {
        if (resourceGroup is not string json)
        {
            throw new InvalidOperationException("Expected string value");
        }

        return JsonSerializer.Deserialize<TOrgName.TIamAzureRepositoryName.Pulumi.Contract.OutputTypes.ResourceGroup>(json)
               ?? throw new InvalidOperationException("Failed to deserialize ResourceGroupType");
    }

}
