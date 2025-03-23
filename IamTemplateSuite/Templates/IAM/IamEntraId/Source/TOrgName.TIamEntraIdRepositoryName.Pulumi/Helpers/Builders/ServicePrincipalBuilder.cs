using Pulumi;
using Pulumi.AzureAD;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

internal class ServicePrincipalBuilder(string resourceName, 
    Application application, CustomResourceOptions? options = null)
{
    public ServicePrincipal Build() =>
        new(resourceName, new()
        {
            ClientId = application.ClientId
        }, options);
}
