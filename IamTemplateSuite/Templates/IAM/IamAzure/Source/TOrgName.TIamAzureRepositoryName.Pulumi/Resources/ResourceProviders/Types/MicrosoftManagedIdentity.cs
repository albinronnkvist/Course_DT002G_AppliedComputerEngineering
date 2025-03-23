using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Builders;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceProviders.Types;

internal sealed class MicrosoftManagedIdentity : ComponentResource
{
    public MicrosoftManagedIdentity(string resourceName, ComponentResourceOptions? options = null) 
        : base("custom:azurenative:MicrosoftManagedIdentity", resourceName, options)
    {
        _ = new ResourceProviderRegistrationCommandBuilder(ResourceNamingConvention.GetResourceName(PulumiResourceType.Command, ResourceProviderType.MicrosoftManagedIdentity.Name), 
                ResourceProviderType.MicrosoftManagedIdentity, new CustomResourceOptions{ Parent = this })
            .Build();
        
        RegisterOutputs();
    }
}
