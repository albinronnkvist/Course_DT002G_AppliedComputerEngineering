using TOrgName.TPlatformPulumiRepositoryName.Core.Helpers;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.Builders;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.RoleAssignments;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using TOrgName.TMicroserviceInfrastructureRepositoryName.Pulumi.StackReferences.TIamAzureRepositoryNameStackReference.Mappers;
using TOrgName.TMicroserviceInfrastructureRepositoryName.Pulumi.StackReferences.TIamEntraIdRepositoryNameStackReference.Mappers;

namespace TOrgName.TMicroserviceInfrastructureRepositoryName.Pulumi;

internal sealed class MainStack : Stack
{
    private static readonly string Environment = Deployment.Instance.StackName;
    
    public MainStack()
    {
        var config = new PulumiConfig();
        const string workload = "TMicroserviceRepositoryGitLabPath";
        
        var TIamEntraIdRepositoryNameStack = StackReferenceHelper.GetStackReference(
            TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.StackDetails.Organization,
            TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.StackDetails.Project,
            TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.StackNames.Main);
        
        var TIamAzureRepositoryNameStack = StackReferenceHelper.GetStackReference(
            TOrgName.TIamAzureRepositoryName.Pulumi.Contract.StackDetails.Organization,
            TOrgName.TIamAzureRepositoryName.Pulumi.Contract.StackDetails.Project, 
            Environment.Equals("dev", StringComparison.OrdinalIgnoreCase) 
                ? TOrgName.TIamAzureRepositoryName.Pulumi.Contract.StackNames.Dev 
                : "prod" // TODO: replace with value when environment is available
        );
        
        var applicationIdentity = TIamEntraIdRepositoryNameStackOutputMapper.GetApplicationIdentityAsync(TIamEntraIdRepositoryNameStack)
            .GetAwaiter().GetResult();

        var resourceGroup = TIamAzureRepositoryNameStackOutputMapper.GetResourceGroupAsync(TIamAzureRepositoryNameStack)
                .GetAwaiter().GetResult();
        
        var keyVault = new AzureKeyVaultBuilder(
            ResourceNamingConvention.GetResourceName(AzureResourceType.KeyVault, workload),
            $"{workload}-{Environment}",
            resourceGroup.ResourceGroupName,
            config.TenantId
        ).Build();
        
       var spAdminRoleAssignment =  new RoleAssignmentBuilder(
                ResourceNamingConvention.GetResourceName(AzureResourceType.RoleAssignment, "spkvadmin"),
                applicationIdentity.ServicePrincipalObjectId,
                PrincipalType.ServicePrincipal, 
                AzureRoleType.KeyVaultAdministrator,
                keyVault.Id)
            .Build();
        
        var miReaderRoleAssignment = new RoleAssignmentBuilder(
                ResourceNamingConvention.GetResourceName(AzureResourceType.RoleAssignment,"mikvsecretsuser"),
                resourceGroup.ManagedIdentityPrincipalId,
                PrincipalType.ServicePrincipal, 
                AzureRoleType.KeyVaultSecretsUser,
                keyVault.Id)
            .Build();
        
        _ = new SecretBuilder(
            ResourceNamingConvention.GetResourceName(AzureResourceType.Secret, "examplesecret"),
            "Example--ExampleSecret",
            config.ExampleSecret,
            resourceGroup.ResourceGroupName,
            keyVault.Name,
            new CustomResourceOptions
            {
                DependsOn = { spAdminRoleAssignment, miReaderRoleAssignment }
            }
        ).Build();

        KeyVaultUri = keyVault.Properties.Apply(x =>
        {
            if (x.VaultUri is null)
            {
                throw new ArgumentNullException(nameof(keyVault), "The vault uri cannot be null.");
            }

            return x.VaultUri;
        });
    }

    [Output]
    public Output<string> KeyVaultUri { get; set; }
}
