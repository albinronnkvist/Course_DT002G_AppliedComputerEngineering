using System.Collections.Immutable;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.DirectoryRoles;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureAD;
using Pulumi.Command.Local;
using Shouldly;
using Xunit;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Test;

public sealed partial class MainStackTests
{
    [Fact]
    public async Task ApplicationIdentitiesComponentResourceShouldExist()
    {
        ImmutableArray<Resource> resources = await TestAsync();

        resources.OfType<ComponentResource>()
            .Single(x => 
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, "application-identities"), StringComparison.Ordinal))
            .ShouldNotBeNull();
    }
    
    [Fact]
    public async Task TIamEntraIdRepositoryNameApplicationIdentityShouldHaveExpectedSetup()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        ApplicationIdentityType applicationIdentityType = ApplicationIdentityType.TIamEntraIdRepositoryName;
        
        ComponentResource componentResource = GetApplicationIdentityComponentResource(resources, applicationIdentityType);
        Application application = GetApplicationIdentityApplication(resources, applicationIdentityType);
        ServicePrincipal servicePrincipal = GetApplicationIdentityServicePrincipal(resources, applicationIdentityType);
        Command flexibleFederatedIdentityCredentialGitLabCommand = GetApplicationIdentityFlexibleFederatedIdentityCredentialGitLabCommand(resources, $"{applicationIdentityType.Name}-gitlab");
        DirectoryRoleAssignment groupsAdministratorDirectoryRoleAssignment = GetApplicationIdentityDirectoryRoleAssignment(resources, applicationIdentityType, DirectoryRoleType.GroupsAdministrator);
            
        componentResource.ShouldNotBeNull();
        (await application.DisplayName.GetValueAsync()).ShouldBe(applicationIdentityType.DisplayName);
        (await servicePrincipal.ClientId.GetValueAsync()).ShouldBe(await application.ClientId.GetValueAsync());
        (await flexibleFederatedIdentityCredentialGitLabCommand.Create.GetValueAsync()).ShouldNotBeNull();
        (await flexibleFederatedIdentityCredentialGitLabCommand.Delete.GetValueAsync()).ShouldNotBeNull();
        groupsAdministratorDirectoryRoleAssignment.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task TIamAzureRepositoryNameApplicationIdentityShouldHaveExpectedSetup()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        ApplicationIdentityType applicationIdentityType = ApplicationIdentityType.TIamAzureRepositoryName;
        
        ComponentResource componentResource = GetApplicationIdentityComponentResource(resources, applicationIdentityType);
        Application application = GetApplicationIdentityApplication(resources, applicationIdentityType);
        ServicePrincipal servicePrincipal = GetApplicationIdentityServicePrincipal(resources, applicationIdentityType);
        Command flexibleFederatedIdentityCredentialGitLabCommand = GetApplicationIdentityFlexibleFederatedIdentityCredentialGitLabCommand(resources, $"{applicationIdentityType.Name}-gitlab");
        
        componentResource.ShouldNotBeNull();
        (await application.DisplayName.GetValueAsync()).ShouldBe(applicationIdentityType.DisplayName);
        (await servicePrincipal.ClientId.GetValueAsync()).ShouldBe(await application.ClientId.GetValueAsync());
        (await flexibleFederatedIdentityCredentialGitLabCommand.Create.GetValueAsync()).ShouldNotBeNull();
        (await flexibleFederatedIdentityCredentialGitLabCommand.Delete.GetValueAsync()).ShouldNotBeNull();
    }
    
    [Fact]
    public async Task TContainerizationRepositoryNameApplicationIdentityShouldHaveExpectedSetup()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        ApplicationIdentityType applicationIdentityType = ApplicationIdentityType.TContainerizationRepositoryName;
        
        ComponentResource componentResource = GetApplicationIdentityComponentResource(resources, applicationIdentityType);
        Application application = GetApplicationIdentityApplication(resources, applicationIdentityType);
        ServicePrincipal servicePrincipal = GetApplicationIdentityServicePrincipal(resources, applicationIdentityType);
        Command flexibleFederatedIdentityCredentialGitLabCommand = GetApplicationIdentityFlexibleFederatedIdentityCredentialGitLabCommand(resources, $"{applicationIdentityType.Name}-gitlab");

        componentResource.ShouldNotBeNull();
        (await application.DisplayName.GetValueAsync()).ShouldBe(applicationIdentityType.DisplayName);
        (await servicePrincipal.ClientId.GetValueAsync()).ShouldBe(await application.ClientId.GetValueAsync());
        (await flexibleFederatedIdentityCredentialGitLabCommand.Create.GetValueAsync()).ShouldNotBeNull();
        (await flexibleFederatedIdentityCredentialGitLabCommand.Delete.GetValueAsync()).ShouldNotBeNull();
    }
    
    [Fact]
    public async Task TMicroserviceRepositoryNameApplicationIdentityShouldHaveExpectedSetup()
    {
        ImmutableArray<Resource> resources = await TestAsync();
        ApplicationIdentityType applicationIdentityType = ApplicationIdentityType.TMicroserviceRepositoryName;
        string ficName = $"{applicationIdentityType.Name}-gitlab";
        string infrastructureFicName = $"{applicationIdentityType.Name}-gitlab-infrastructure";
        string infrastructureContainerizationDeploymentFicName = $"{applicationIdentityType.Name}-gitlab-containerization-deployment";
        
        ComponentResource componentResource = GetApplicationIdentityComponentResource(resources, applicationIdentityType);
        Application application = GetApplicationIdentityApplication(resources, applicationIdentityType);
        ServicePrincipal servicePrincipal = GetApplicationIdentityServicePrincipal(resources, applicationIdentityType);
        Command flexibleFederatedIdentityCredentialGitLabCommand = GetApplicationIdentityFlexibleFederatedIdentityCredentialGitLabCommand(resources, ficName);
        Command flexibleFederatedIdentityCredentialGitLabCommandInfrastructure = GetApplicationIdentityFlexibleFederatedIdentityCredentialGitLabCommand(resources, infrastructureFicName);
        Command flexibleFederatedIdentityCredentialGitLabCommandContainerization = GetApplicationIdentityFlexibleFederatedIdentityCredentialGitLabCommand(resources, infrastructureContainerizationDeploymentFicName);
        
        componentResource.ShouldNotBeNull();
        (await application.DisplayName.GetValueAsync()).ShouldBe(applicationIdentityType.DisplayName);
        (await servicePrincipal.ClientId.GetValueAsync()).ShouldBe(await application.ClientId.GetValueAsync());
        (await flexibleFederatedIdentityCredentialGitLabCommand.Create.GetValueAsync()).ShouldNotBeNull();
        (await flexibleFederatedIdentityCredentialGitLabCommand.Delete.GetValueAsync()).ShouldNotBeNull();
        (await flexibleFederatedIdentityCredentialGitLabCommandInfrastructure.Create.GetValueAsync()).ShouldNotBeNull();
        (await flexibleFederatedIdentityCredentialGitLabCommandInfrastructure.Delete.GetValueAsync()).ShouldNotBeNull();
        (await flexibleFederatedIdentityCredentialGitLabCommandContainerization.Create.GetValueAsync()).ShouldNotBeNull();
        (await flexibleFederatedIdentityCredentialGitLabCommandContainerization.Delete.GetValueAsync()).ShouldNotBeNull();
    }
    
    private static ComponentResource GetApplicationIdentityComponentResource(ImmutableArray<Resource> resources, ApplicationIdentityType applicationIdentityType) =>
        resources
            .OfType<ComponentResource>()
            .Single(x =>
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(PulumiResourceType.ComponentResource, applicationIdentityType.Name), StringComparison.Ordinal));

    private static Application GetApplicationIdentityApplication(ImmutableArray<Resource> resources, ApplicationIdentityType applicationIdentityType) =>
        resources
            .OfType<Application>()
            .Single(x =>
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(EntraIdResourceType.Application, applicationIdentityType.Name), StringComparison.Ordinal));
    
    private static ServicePrincipal GetApplicationIdentityServicePrincipal(ImmutableArray<Resource> resources, ApplicationIdentityType applicationIdentityType) =>
        resources
            .OfType<ServicePrincipal>()
            .Single(x =>
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(EntraIdResourceType.ServicePrincipal, applicationIdentityType.Name), StringComparison.Ordinal));
    
    private static Command GetApplicationIdentityFlexibleFederatedIdentityCredentialGitLabCommand(ImmutableArray<Resource> resources, string ficName) =>
        resources
            .OfType<Command>()
            .Single(x =>
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(PulumiResourceType.Command, ficName), StringComparison.Ordinal));
    
    private static DirectoryRoleAssignment GetApplicationIdentityDirectoryRoleAssignment(ImmutableArray<Resource> resources, ApplicationIdentityType applicationIdentityType, DirectoryRoleType directoryRoleType) =>
        resources
            .OfType<DirectoryRoleAssignment>()
            .Single(x => 
                x.GetResourceName().Equals(ResourceNamingConvention.GetResourceName(EntraIdResourceType.DirectoryRoleAssignment, $"{applicationIdentityType.Name}-{directoryRoleType.Name}"), StringComparison.Ordinal));
}
