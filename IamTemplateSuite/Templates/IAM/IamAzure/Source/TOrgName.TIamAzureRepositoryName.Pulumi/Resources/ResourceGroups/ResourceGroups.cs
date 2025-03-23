using System.Collections.Immutable;
using TOrgName.TIamAzureRepositoryName.Pulumi.Extensions;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Identities.WorkloadIdentities;
using TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceGroups.Types;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceGroups;

internal sealed class ResourceGroups : ComponentResource
{
    public ResourceGroups(string resourceName, ImmutableDictionary<ApplicationIdentityType, ApplicationIdentity> applicationIdentities, ComponentResourceOptions options)
        : base("custom:azurenative:ResourceGroups", resourceName, options)
    {
        var resourceGroups = new Dictionary<string, IResourceGroup>
        {
            { 
                ResourceGroupType.TContainerizationRepositoryName.Name, 
                new TContainerizationRepositoryName(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, ResourceGroupType.TContainerizationRepositoryName.Name), 
                    applicationIdentities.GetValueOrThrow(ApplicationIdentityType.TContainerizationRepositoryName), new ComponentResourceOptions
                    {
                        DependsOn = options.DependsOn,
                        Parent = this
                    })
            },
            { 
                ResourceGroupType.TMicroserviceRepositoryName.Name, 
                new TMicroserviceRepositoryName(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, ResourceGroupType.TMicroserviceRepositoryName.Name), 
                    applicationIdentities.GetValueOrThrow(ApplicationIdentityType.TMicroserviceRepositoryName), new ComponentResourceOptions
                    {
                        DependsOn = options.DependsOn,
                        Parent = this
                    })
            }
        };
        
        ResourceGroupMap = Output.Create(resourceGroups);
        RegisterOutputs(new Dictionary<string, object?>
        {
            { "resourceGroupMap", ResourceGroupMap }
        });
    }
    
    public Output<Dictionary<string, IResourceGroup>> ResourceGroupMap { get; }
}
