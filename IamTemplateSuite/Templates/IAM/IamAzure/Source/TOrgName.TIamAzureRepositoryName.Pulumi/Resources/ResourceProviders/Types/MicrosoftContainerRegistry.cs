using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Builders;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceProviders.Types;

internal sealed class MicrosoftContainerRegistry : ComponentResource
{
    public MicrosoftContainerRegistry(string resourceName, ComponentResourceOptions? options = null) 
        : base("custom:azurenative:MicrosoftContainerRegistry", resourceName, options)
    {
        _ = new ResourceProviderRegistrationCommandBuilder(ResourceNamingConvention.GetResourceName(PulumiResourceType.Command, ResourceProviderType.MicrosoftContainerRegistry.Name), 
                ResourceProviderType.MicrosoftContainerRegistry, new CustomResourceOptions{ Parent = this })
            .Build();
        
        RegisterOutputs();
    }
}
