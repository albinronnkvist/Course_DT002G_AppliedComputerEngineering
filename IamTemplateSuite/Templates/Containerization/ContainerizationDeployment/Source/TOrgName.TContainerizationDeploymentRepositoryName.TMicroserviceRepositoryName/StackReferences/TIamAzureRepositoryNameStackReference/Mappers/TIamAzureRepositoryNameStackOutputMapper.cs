using System.Collections.Immutable;
using System.Text.Json;
using Pulumi;

namespace TOrgName.TContainerizationDeploymentRepositoryName.TMicroserviceRepositoryName.StackReferences.TIamAzureRepositoryNameStackReference.Mappers;

internal static class TIamAzureRepositoryNameStackOutputMapper
{
    internal static async Task<ResourceGroup> GetResourceGroupAsync(StackReference stackReference)
    {
        StackReferenceOutputDetails outputDetails = await stackReference
            .GetOutputDetailsAsync(TIamAzureRepositoryName.Pulumi.Contract.StackOutputs.ResourceGroups);
        
        if (outputDetails.Value is not ImmutableDictionary<string, object> resourceGroups)
        {
            throw new InvalidOperationException($"Stack reference output '{TIamAzureRepositoryName.Pulumi.Contract.StackOutputs.ResourceGroups}' was not a valid type.");
        }
        
        if (!resourceGroups.TryGetValue(
                TIamAzureRepositoryName.Pulumi.Contract.Enums.ResourceGroupType.TMicroserviceRepositoryName.ToString(),
                out object? TMicroserviceRepositoryNameResourceGroup))
        {
            throw new KeyNotFoundException($"ResourceGroupType '{TIamAzureRepositoryName.Pulumi.Contract.Enums.ResourceGroupType.TMicroserviceRepositoryName}' key not found.");
        }

        return MapResourceGroup(ParseResourceGroup(TMicroserviceRepositoryNameResourceGroup));
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
