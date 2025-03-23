using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications.Types;

internal class TMicroserviceRepositoryName : ComponentResource, IApplicationIdentity
{
    public TMicroserviceRepositoryName(string resourceName, bool isFlexibleFic, ComponentResourceOptions? options = null)
        : base("custom:azuread:TMicroserviceRepositoryName", resourceName, options)
    {
        ApplicationIdentityType applicationIdentityType = ApplicationIdentityType.TMicroserviceRepositoryName;

        Application application = new ApplicationBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.Application, applicationIdentityType.Name), 
                applicationIdentityType, new CustomResourceOptions { Parent = this })
            .Build();
        
        string infrastructureRepositoryPath = "TMicroserviceInfrastructureRepositoryGitLabPath";
        const string containerizationDeploymentRepositoryPath = "TContainerizationDeploymentRepositoryGitLabPath";
        string ficName = $"{applicationIdentityType.Name}-gitlab";
        string infrastructureFicName = $"{applicationIdentityType.Name}-gitlab-infrastructure";
        string containerizationDeploymentFicName = $"{applicationIdentityType.Name}-gitlab-containerization-deployment";
        if (isFlexibleFic)
        {
            _ = new FlexibleFederatedIdentityCredentialGitLabCommandBuilder(ResourceNamingConvention.GetResourceName(PulumiResourceType.Command, ficName),
                    ficName, applicationIdentityType.RepositoryPath, application.ObjectId, new CustomResourceOptions { Parent = this })
                .Build();
            
            _ = new FlexibleFederatedIdentityCredentialGitLabCommandBuilder(ResourceNamingConvention.GetResourceName(PulumiResourceType.Command, infrastructureFicName),
                    infrastructureFicName, infrastructureRepositoryPath, application.ObjectId, new CustomResourceOptions { Parent = this })
                .Build();
            
            _ = new FlexibleFederatedIdentityCredentialGitLabCommandBuilder(ResourceNamingConvention.GetResourceName(PulumiResourceType.Command, containerizationDeploymentFicName),
                    containerizationDeploymentFicName, containerizationDeploymentRepositoryPath, application.ObjectId, new CustomResourceOptions { Parent = this })
                .Build();
        }
        else
        {
            _ = new ApplicationFederatedIdentityCredentialGitLabBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.FederatedIdentityCredential, ficName),
                ficName, applicationIdentityType.RepositoryPath, application.ObjectId, new CustomResourceOptions { Parent = this });
            
            _ = new ApplicationFederatedIdentityCredentialGitLabBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.FederatedIdentityCredential, infrastructureFicName),
                infrastructureFicName, infrastructureRepositoryPath, application.ObjectId, new CustomResourceOptions { Parent = this });
            
            _ = new ApplicationFederatedIdentityCredentialGitLabBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.FederatedIdentityCredential, containerizationDeploymentFicName),
                containerizationDeploymentFicName, containerizationDeploymentRepositoryPath, application.ObjectId, new CustomResourceOptions { Parent = this });
        }
        
        ServicePrincipal servicePrincipal = new ServicePrincipalBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.ServicePrincipal, applicationIdentityType.Name), 
                application, new CustomResourceOptions { Parent = this })
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
