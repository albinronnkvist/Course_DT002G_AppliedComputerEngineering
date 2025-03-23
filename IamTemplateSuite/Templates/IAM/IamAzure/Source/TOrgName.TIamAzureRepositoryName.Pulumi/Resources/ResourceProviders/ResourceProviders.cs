using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceProviders.Types;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceProviders;

internal sealed class ResourceProviders : ComponentResource
{
    public ResourceProviders(string resourceName, ComponentResourceOptions? options = null)
        : base("custom:azurenative:ResourceProviders", resourceName, options)
    {
        _ = new MicrosoftApp(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, ResourceProviderType.MicrosoftApp.Name), 
            new ComponentResourceOptions { Parent = this });
        
        _ = new MicrosoftKeyVault(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, ResourceProviderType.MicrosoftKeyVault.Name), 
            new ComponentResourceOptions { Parent = this });
        
        _ = new MicrosoftContainerRegistry(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, ResourceProviderType.MicrosoftContainerRegistry.Name), 
            new ComponentResourceOptions { Parent = this });
        
        _ = new MicrosoftManagedIdentity(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, ResourceProviderType.MicrosoftManagedIdentity.Name), 
            new ComponentResourceOptions { Parent = this });
        
        RegisterOutputs();
    }
}
