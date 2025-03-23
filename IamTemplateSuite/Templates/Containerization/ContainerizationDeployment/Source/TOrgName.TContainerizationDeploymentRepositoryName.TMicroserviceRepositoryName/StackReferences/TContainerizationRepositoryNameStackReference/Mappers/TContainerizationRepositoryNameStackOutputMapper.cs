using Pulumi;

namespace TOrgName.TContainerizationDeploymentRepositoryName.TMicroserviceRepositoryName.StackReferences.TContainerizationRepositoryNameStackReference.Mappers;

internal static class TContainerizationRepositoryNameStackOutputMapper
{
    internal static async Task<string> GetContainerAppsEnvironmentIdAsync(StackReference stackReference)
    {
        StackReferenceOutputDetails outputDetails = await stackReference
            .GetOutputDetailsAsync(TContainerizationRepositoryName.Pulumi.Contract.StackOutputs.ContainerAppsEnvironmentId);
        
        return outputDetails.Value is string containerAppsEnvironmentId && !string.IsNullOrWhiteSpace(containerAppsEnvironmentId)
            ? containerAppsEnvironmentId
            : throw new InvalidOperationException("ContainerAppsEnvironmentId is either null, empty, or not a valid string.");
    }
    
    internal static async Task<string> GetContainerRegistryServerUrlAsync(StackReference stackReference)
    {
        StackReferenceOutputDetails outputDetails = await stackReference
            .GetOutputDetailsAsync(TContainerizationRepositoryName.Pulumi.Contract.StackOutputs.ContainerRegistryServerUrl);
        
        return outputDetails.Value is string containerRegistryServerUrl && !string.IsNullOrWhiteSpace(containerRegistryServerUrl)
            ? containerRegistryServerUrl
            : throw new InvalidOperationException("ContainerRegistryServerUrl is either null, empty, or not a valid string.");
    }
}
