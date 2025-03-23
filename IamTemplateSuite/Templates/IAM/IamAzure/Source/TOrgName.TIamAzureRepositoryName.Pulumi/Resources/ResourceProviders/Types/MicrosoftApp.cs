using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Builders;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceProviders.Types;

internal sealed class MicrosoftApp : ComponentResource
{
    public MicrosoftApp(string resourceName, ComponentResourceOptions? options = null) 
        : base("custom:azurenative:MicrosoftApp", resourceName, options)
    {
        _ = new ResourceProviderRegistrationCommandBuilder(ResourceNamingConvention.GetResourceName(PulumiResourceType.Command, ResourceProviderType.MicrosoftApp.Name), 
                ResourceProviderType.MicrosoftApp, new CustomResourceOptions{ Parent = this })
            .Build();
        
        RegisterOutputs();
    }
}
