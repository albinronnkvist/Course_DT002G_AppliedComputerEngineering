using System.Collections.Immutable;
using Pulumi;

namespace TOrgName.TContainerizationRepositoryName.Pulumi.StackReferences.TIamAzureRepositoryNameStackReference.Mappers;

internal static class TIamAzureRepositoryNameStackOutputMapper
{
    internal static async Task<ImmutableDictionary<ResourceGroupType, ResourceGroup>> GetResourceGroupsAsync(StackReference stackReference)
    {
        var outputDetails = await stackReference
            .GetOutputDetailsAsync(TOrgName.TIamAzureRepositoryName.Pulumi.Contract.StackOutputs.ResourceGroups);
        
        if (outputDetails.Value is not ImmutableDictionary<string, object> resourceGroups)
        {
            throw new InvalidOperationException($"Stack reference output '{TOrgName.TIamAzureRepositoryName.Pulumi.Contract.StackOutputs.ResourceGroups}' was not a valid type.");
        }

        return ResourceGroupMapper.Map(resourceGroups);
    }
}
