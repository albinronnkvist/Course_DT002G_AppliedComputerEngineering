using Pulumi;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;

public sealed class ResourceGroupBuilder(string resourceName, string resourceGroupName, CustomResourceOptions? options = null)
{
    public Pulumi.AzureNative.Resources.ResourceGroup Build() =>
        new(resourceName, new()
        {
            ResourceGroupName = resourceGroupName
        }, options);
}
