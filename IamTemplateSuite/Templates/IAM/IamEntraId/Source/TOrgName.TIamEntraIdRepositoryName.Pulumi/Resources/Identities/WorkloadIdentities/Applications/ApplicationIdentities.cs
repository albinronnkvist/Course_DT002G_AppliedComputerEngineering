using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications.Types;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications;

internal class ApplicationIdentities : ComponentResource
{
    public ApplicationIdentities(string resourceName, Output<Dictionary<string, DirectoryRole>> directoryRoles, ComponentResourceOptions? options = null)
        : base("custom:azuread:ApplicationIdentities", resourceName, options)
    {
        var applicationIdentities = new Dictionary<string, IApplicationIdentity>
        {
            { ApplicationIdentityType.TIamEntraIdRepositoryName.Name, new Types.TIamEntraIdRepositoryName(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, ApplicationIdentityType.TIamEntraIdRepositoryName.Name), 
                directoryRoles,true, new ComponentResourceOptions { Parent = this }) 
            },
            { ApplicationIdentityType.TIamAzureRepositoryName.Name, new TIamAzureRepositoryName(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, ApplicationIdentityType.TIamAzureRepositoryName.Name), 
                true, new ComponentResourceOptions { Parent = this }) 
            },
            { ApplicationIdentityType.TContainerizationRepositoryName.Name, new TContainerizationRepositoryName(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, ApplicationIdentityType.TContainerizationRepositoryName.Name), 
                true, new ComponentResourceOptions { Parent = this }) 
            },
            { ApplicationIdentityType.TMicroserviceRepositoryName.Name, new TMicroserviceRepositoryName(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, ApplicationIdentityType.TMicroserviceRepositoryName.Name), 
                true, new ComponentResourceOptions { Parent = this }) 
            }
        };

        ApplicationIdentityMap = Output.Create(applicationIdentities);
        RegisterOutputs(new Dictionary<string, object?>
        {
            { "applicationIdentityMap", ApplicationIdentityMap }
        });
    }
    
    public Output<Dictionary<string, IApplicationIdentity>> ApplicationIdentityMap { get; }
}
