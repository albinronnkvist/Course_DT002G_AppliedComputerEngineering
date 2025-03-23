using Pulumi;
using Pulumi.AzureAD;
using Pulumi.AzureAD.Inputs;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Permissions;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

internal class ApplicationBuilder(string resourceName, 
    ApplicationIdentityType applicationIdentityType, CustomResourceOptions? options = null)
{
    private readonly InputList<ApplicationRequiredResourceAccessArgs> _apiPermissions = new();
    
    public ApplicationBuilder WithApiPermissions(IReadOnlyCollection<ApiPermissionsByApi> permissionsByApi)
    {
        foreach (ApiPermissionsByApi permissions in permissionsByApi)
        {
            _apiPermissions.Add(new ApplicationRequiredResourceAccessArgs
            {
                ResourceAppId = permissions.Api.ResourceAppId,
                ResourceAccesses = new InputList<ApplicationRequiredResourceAccessResourceAccessArgs> 
                { 
                    permissions.Permissions.Select(p => new ApplicationRequiredResourceAccessResourceAccessArgs
                    {
                        Id = p.Permission.PermissionId,
                        Type = p.PermissionType.Name
                    }).ToList() 
                }
            });
        }
        
        return this;
    }
    
    public Application Build() =>
        new(resourceName, new()
        {
            DisplayName = applicationIdentityType.DisplayName,
            RequiredResourceAccesses = _apiPermissions
        }, options);
}
