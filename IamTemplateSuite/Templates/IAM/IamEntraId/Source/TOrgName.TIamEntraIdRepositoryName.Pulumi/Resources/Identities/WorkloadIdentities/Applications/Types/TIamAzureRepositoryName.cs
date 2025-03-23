using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications.Types;

internal class TIamAzureRepositoryName : ComponentResource, IApplicationIdentity
{
    public TIamAzureRepositoryName(string resourceName, bool isFlexibleFic, ComponentResourceOptions? options = null)
        : base("custom:azuread:TIamAzureRepositoryName", resourceName, options)
    {
        ApplicationIdentityType applicationIdentityType = ApplicationIdentityType.TIamAzureRepositoryName;

        Application application = new ApplicationBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.Application, applicationIdentityType.Name), 
                applicationIdentityType, new CustomResourceOptions { Parent = this })
            .Build();
        
        if (isFlexibleFic)
        {
            _ = new FlexibleFederatedIdentityCredentialGitLabCommandBuilder(ResourceNamingConvention.GetResourceName(PulumiResourceType.Command, $"{applicationIdentityType.Name}-gitlab"),
                    $"{applicationIdentityType.Name}-gitlab", applicationIdentityType.RepositoryPath, application.ObjectId, new CustomResourceOptions { Parent = this })
                .Build();
        }
        else
        {
            _ = new ApplicationFederatedIdentityCredentialGitLabBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.FederatedIdentityCredential, applicationIdentityType.Name),
                $"{applicationIdentityType.Name}-gitlab", applicationIdentityType.RepositoryPath, application.ObjectId, new CustomResourceOptions { Parent = this });
        }
        
        ServicePrincipal servicePrincipal = new ServicePrincipalBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.ServicePrincipal, 
                applicationIdentityType.Name), application, new CustomResourceOptions { Parent = this })
            .Build();

        ApplicationClientId = application.ClientId.Apply(id => id);
        ServicePrincipalObjectId = servicePrincipal.ObjectId.Apply(id => id);
        RegisterOutputs(new Dictionary<string, object?>
        {
            { "applicationClientId", ApplicationClientId },
            { "servicePrincipalObjectId", ServicePrincipalObjectId }
        });
    }

    public Output<string> ApplicationClientId { get; }
    public Output<string> ServicePrincipalObjectId { get; }
}
