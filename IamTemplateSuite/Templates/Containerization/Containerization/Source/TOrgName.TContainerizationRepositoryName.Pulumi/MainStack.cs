using TOrgName.TContainerizationRepositoryName.Pulumi.Extensions;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract;
using TOrgName.TPlatformPulumiRepositoryName.Core.Helpers;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.RoleAssignments;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.ContainerRegistry;
using Pulumi.AzureNative.ContainerRegistry.Inputs;
using TOrgName.TContainerizationRepositoryName.Pulumi.StackReferences.TIamAzureRepositoryNameStackReference;
using TOrgName.TContainerizationRepositoryName.Pulumi.StackReferences.TIamAzureRepositoryNameStackReference.Mappers;
using TOrgName.TContainerizationRepositoryName.Pulumi.StackReferences.TIamEntraIdRepositoryNameStackReference;
using TOrgName.TContainerizationRepositoryName.Pulumi.StackReferences.TIamEntraIdRepositoryNameStackReference.Mappers;
using Deployment = Pulumi.Deployment;
using AzureNative = Pulumi.AzureNative;
using StackDetails = TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.StackDetails;
using StackOutputs = TOrgName.TContainerizationRepositoryName.Pulumi.Contract.StackOutputs;

namespace TOrgName.TContainerizationRepositoryName.Pulumi;

internal sealed class MainStack : Stack
{
    private static readonly string Environment = Deployment.Instance.StackName;
    
    public MainStack()
    {  
        var TIamEntraIdRepositoryNameMainStack = StackReferenceHelper.GetStackReference(
            StackDetails.Organization,
            StackDetails.Project, 
            StackNames.Main);
        
        var TIamAzureRepositoryNameStack = StackReferenceHelper.GetStackReference(
            TOrgName.TIamAzureRepositoryName.Pulumi.Contract.StackDetails.Organization,
            TOrgName.TIamAzureRepositoryName.Pulumi.Contract.StackDetails.Project, 
            Environment.Equals("dev", StringComparison.OrdinalIgnoreCase) 
                ? TOrgName.TIamAzureRepositoryName.Pulumi.Contract.StackNames.Dev 
                : "prod" // TODO: replace with value when stack is available
        );
        
        var applicationIdentities = TIamEntraIdRepositoryNameStackOutputMapper.GetApplicationIdentitiesAsync(TIamEntraIdRepositoryNameMainStack)
            .GetAwaiter().GetResult();
        
        var resourceGroups = TIamAzureRepositoryNameStackOutputMapper.GetResourceGroupsAsync(TIamAzureRepositoryNameStack)
            .GetAwaiter().GetResult();
        
        var TContainerizationRepositoryNameResourceGroup = resourceGroups.GetValueOrThrow(ResourceGroupType.InfrastructureContainerization);
        
        var containerRegistry = new Registry(ResourceNamingConvention.GetResourceName(AzureResourceType.AzureContainerRegistry, "TOrgName"),
            new RegistryArgs
        {
            RegistryName = $"TOrgName{Environment}",
            ResourceGroupName = TContainerizationRepositoryNameResourceGroup.ResourceGroupName,
            Sku = new SkuArgs { Name = SkuName.Basic }
        });
        
        var containerAppsEnv = new AzureNative.App.ManagedEnvironment(ResourceNamingConvention.GetResourceName(AzureResourceType.ContainerAppsEnvironment, "TOrgName"), new()
        {
            EnvironmentName = $"TOrgName-{Environment}",
            ResourceGroupName = TContainerizationRepositoryNameResourceGroup.ResourceGroupName
        });
        
        // Give roles to TMicroserviceRepositoryName
        var TMicroserviceRepositoryNameApplicationIdentity = applicationIdentities.GetValueOrThrow(ApplicationIdentityType.ExampleMicroservice);
        var TMicroserviceRepositoryNameResourceGroup = resourceGroups.GetValueOrThrow(ResourceGroupType.ExampleMicroservice);
        const string TMicroserviceRepositoryNameWorkloadName = "TMicroserviceRepositoryGitLabPath";
        _ =  new RoleAssignmentBuilder(
                ResourceNamingConvention.GetResourceName(AzureResourceType.RoleAssignment, $"{TMicroserviceRepositoryNameWorkloadName}-spacrpush"),
                TMicroserviceRepositoryNameApplicationIdentity.ServicePrincipalObjectId,
                PrincipalType.ServicePrincipal, 
                AzureRoleType.AcrPush,
                containerRegistry.Id)
            .Build();
        
        _ =  new RoleAssignmentBuilder(
                ResourceNamingConvention.GetResourceName(AzureResourceType.RoleAssignment, $"{TMicroserviceRepositoryNameWorkloadName}-miacrpull"),
                TMicroserviceRepositoryNameResourceGroup.ManagedIdentityPrincipalId,
                PrincipalType.ServicePrincipal, 
                AzureRoleType.AcrPull,
                containerRegistry.Id)
            .Build();
        
       _ =  new RoleAssignmentBuilder(
               ResourceNamingConvention.GetResourceName(AzureResourceType.RoleAssignment, $"{TMicroserviceRepositoryNameWorkloadName}-spcaecontributor"),
               TMicroserviceRepositoryNameApplicationIdentity.ServicePrincipalObjectId,
               PrincipalType.ServicePrincipal, 
               AzureRoleType.ContainerAppsContributor,
               containerAppsEnv.Id)
           .Build();

        ContainerRegistryServerUrl = containerRegistry.LoginServer;
        ContainerAppsEnvironmentId = containerAppsEnv.Id;
    }

    [Output(StackOutputs.ContainerRegistryServerUrl)]
    public Output<string> ContainerRegistryServerUrl { get; set; }
    
    [Output(StackOutputs.ContainerAppsEnvironmentId)]
    public Output<string> ContainerAppsEnvironmentId { get; set; }
}
