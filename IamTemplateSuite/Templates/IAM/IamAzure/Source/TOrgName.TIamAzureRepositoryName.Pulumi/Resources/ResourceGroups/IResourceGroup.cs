using Pulumi;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceGroups;

internal interface IResourceGroup
{
    public Output<string> ResourceGroupName { get; }
    public Output<string> ManagedIdentityId { get; }
    public Output<string> ManagedIdentityClientId { get; }
    public Output<string> ManagedIdentityPrincipalId { get; }
}
