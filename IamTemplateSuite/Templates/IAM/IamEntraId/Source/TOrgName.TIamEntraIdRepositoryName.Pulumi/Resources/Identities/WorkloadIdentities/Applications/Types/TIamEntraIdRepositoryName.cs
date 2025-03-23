using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;
using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.DirectoryRoles;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Permissions;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications.Types;

internal class TIamEntraIdRepositoryName : ComponentResource, IApplicationIdentity
{
    public TIamEntraIdRepositoryName(string resourceName, 
        Output<Dictionary<string, DirectoryRole>> directoryRoles, bool isFlexibleFic, ComponentResourceOptions? options = null)
        : base("custom:azuread:TIamEntraIdRepositoryName", resourceName, options)
    {
        ApplicationIdentityType applicationIdentityType = ApplicationIdentityType.TIamEntraIdRepositoryName;
        
        var apiPermissionsMicrosoftGraph = new ApiPermissionsByApi(Api.MicrosoftGraph, new []
        {
            new ApiPermissionWithType(MicrosoftGraphApiPermission.ApplicationReadWriteAll, ApiPermissionType.Application),
            new ApiPermissionWithType(MicrosoftGraphApiPermission.DirectoryReadWriteAll, ApiPermissionType.Application),
            new ApiPermissionWithType(MicrosoftGraphApiPermission.RoleManagementReadWriteDirectory, ApiPermissionType.Application),
            new ApiPermissionWithType(MicrosoftGraphApiPermission.UserReadWriteAll, ApiPermissionType.Application)
        });
        var apiPermissions = new List<ApiPermissionsByApi> { apiPermissionsMicrosoftGraph };

        Application application = new ApplicationBuilder(ResourceNamingConvention.GetResourceName(EntraIdResourceType.Application, applicationIdentityType.Name), 
                applicationIdentityType, new CustomResourceOptions { Parent = this })
            .WithApiPermissions(apiPermissions)
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

        directoryRoles.Apply(roles =>
        {
            if (!roles.TryGetValue(DirectoryRoleType.GroupsAdministrator.Name, out DirectoryRole? groupsAdministrator))
            {
                throw new KeyNotFoundException($"Directory role '{DirectoryRoleType.GroupsAdministrator.Name}' not found");
            }
            ArgumentNullException.ThrowIfNull(groupsAdministrator);

            _ = new DirectoryRoleAssignmentBuilder(
                ResourceNamingConvention.GetResourceName(EntraIdResourceType.DirectoryRoleAssignment, $"{applicationIdentityType.Name}-{DirectoryRoleType.GroupsAdministrator.Name}"),
                groupsAdministrator.TemplateId, servicePrincipal.ObjectId, new CustomResourceOptions { Parent = this })
                .Build();

            return roles;
        });
        
        foreach (ApiPermissionsByApi permissions in apiPermissions)
        {
            Output<GetServicePrincipalResult> apiServicePrincipal = GetServicePrincipal.Invoke(new GetServicePrincipalInvokeArgs
            {
                ClientId = permissions.Api.ResourceAppId
            }, new InvokeOutputOptions
            {
                DependsOn = servicePrincipal
            });
            
            string[] permissionIds = permissions.Permissions
                .Select(p => p.Permission.PermissionId)
                .ToArray();

            foreach (string permissionId in permissionIds)
            {
                _ = new AppRoleAssignment(ResourceNamingConvention.GetResourceName(EntraIdResourceType.AppRoleAssignment, $"{applicationIdentityType.Name}-{permissionId}"), 
                    new AppRoleAssignmentArgs
                    {
                        AppRoleId = permissionId,
                        PrincipalObjectId = servicePrincipal.ObjectId,
                        ResourceObjectId = apiServicePrincipal.Apply(x => x.ObjectId),
                    }, new CustomResourceOptions { Parent = this });   
            }
        }

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

