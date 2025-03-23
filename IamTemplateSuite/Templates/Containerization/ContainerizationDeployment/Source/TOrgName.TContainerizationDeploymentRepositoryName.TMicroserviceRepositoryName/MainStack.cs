using TOrgName.TContainerizationDeploymentRepositoryName.TMicroserviceRepositoryName.StackReferences.TContainerizationRepositoryNameStackReference.Mappers;
using TOrgName.TContainerizationDeploymentRepositoryName.TMicroserviceRepositoryName.StackReferences.TIamAzureRepositoryNameStackReference;
using TOrgName.TContainerizationDeploymentRepositoryName.TMicroserviceRepositoryName.StackReferences.TIamAzureRepositoryNameStackReference.Mappers;
using TOrgName.TPlatformPulumiRepositoryName.Core.Helpers;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.App.Inputs;

namespace TOrgName.TContainerizationDeploymentRepositoryName.TMicroserviceRepositoryName;

internal sealed class MainStack : Stack
{
    private static readonly string Environment = Deployment.Instance.StackName;
    
    public MainStack()
    {
        var config = new PulumiConfig();
        
        StackReference TIamAzureRepositoryNameStack = StackReferenceHelper.GetStackReference(
            TIamAzureRepositoryName.Pulumi.Contract.StackDetails.Organization,
            TIamAzureRepositoryName.Pulumi.Contract.StackDetails.Project,
            Environment.Equals("dev", StringComparison.OrdinalIgnoreCase)
                ? TIamAzureRepositoryName.Pulumi.Contract.StackNames.Dev
                : "prod" // TODO: replace with value when stack is available)
        );
        
        StackReference TContainerizationRepositoryNameStack = StackReferenceHelper.GetStackReference(
            TContainerizationRepositoryName.Pulumi.Contract.StackDetails.Organization, 
            TContainerizationRepositoryName.Pulumi.Contract.StackDetails.Project,
            Environment.Equals("dev", StringComparison.OrdinalIgnoreCase) 
                ? TContainerizationRepositoryName.Pulumi.Contract.StackNames.Dev 
                : "prod" // TODO: replace with value when stack is available
        );
        
        ResourceGroup resourceGroup = TIamAzureRepositoryNameStackOutputMapper.GetResourceGroupAsync(TIamAzureRepositoryNameStack)
            .GetAwaiter().GetResult();
        string containerAppsEnvironmentId = TContainerizationRepositoryNameStackOutputMapper.GetContainerAppsEnvironmentIdAsync(TContainerizationRepositoryNameStack)
            .GetAwaiter().GetResult();
        string containerRegistryServerUrl = TContainerizationRepositoryNameStackOutputMapper.GetContainerRegistryServerUrlAsync(TContainerizationRepositoryNameStack)
            .GetAwaiter().GetResult();
        
        #pragma warning disable CA1308
        string workload = "TMicroserviceRepositoryName".ToLowerInvariant();
        #pragma warning restore CA1308
        string apiWorkload = $"{workload}-api";
        _ = new ContainerApp(ResourceNamingConvention.GetResourceName(AzureResourceType.ContainerApp, "api"), new ContainerAppArgs
        {
            ContainerAppName = apiWorkload,
            ResourceGroupName = resourceGroup.ResourceGroupName,
            ManagedEnvironmentId = containerAppsEnvironmentId,
            Configuration = new ConfigurationArgs
            {
                Ingress = new IngressArgs
                {
                    External = true,
                    TargetPort = 8080,
                    Transport = IngressTransportMethod.Http
                },
                Registries =
                {
                    new RegistryCredentialsArgs
                    {
                        Server = containerRegistryServerUrl,
                        Identity = resourceGroup.ManagedIdentityId
                    }
                }
            },
            Identity = new ManagedServiceIdentityArgs
            {
                Type = ManagedServiceIdentityType.UserAssigned,
                UserAssignedIdentities = new InputList<string>()
                {
                    resourceGroup.ManagedIdentityId
                }
            },
            Template = new TemplateArgs
            {
                Containers =
                {
                    new ContainerArgs
                    {
                        Name = apiWorkload,
                        Image = $"{containerRegistryServerUrl}/{apiWorkload}:{config.ImageTagApi}",
                        Resources = new ContainerResourcesArgs
                        {
                            Cpu = 0.5,
                            Memory = "1Gi"
                        },
                        Env = new List<EnvironmentVarArgs>
                        {
                            new()
                            {
                                Name = "DOTNET_ENVIRONMENT",
                                Value = config.DotnetEnvironment
                            }
                        }
                    }
                },
                Scale = new ScaleArgs
                {
                    MinReplicas = 1,
                    MaxReplicas = 3
                }
            }
        });
    }
}
