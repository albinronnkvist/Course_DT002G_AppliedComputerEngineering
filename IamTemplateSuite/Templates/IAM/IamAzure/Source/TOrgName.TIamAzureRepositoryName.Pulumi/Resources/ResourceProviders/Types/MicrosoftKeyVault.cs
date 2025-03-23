using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Builders;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceProviders.Types;

internal sealed class MicrosoftKeyVault : ComponentResource
{
    public MicrosoftKeyVault(string resourceName, ComponentResourceOptions? options = null) 
        : base("custom:azurenative:MicrosoftKeyVault", resourceName, options)
    {
        _ = new ResourceProviderRegistrationCommandBuilder(ResourceNamingConvention.GetResourceName(PulumiResourceType.Command, ResourceProviderType.MicrosoftKeyVault.Name), 
                ResourceProviderType.MicrosoftKeyVault, new CustomResourceOptions{ Parent = this })
            .Build();
        
        RegisterOutputs();
    }
}
