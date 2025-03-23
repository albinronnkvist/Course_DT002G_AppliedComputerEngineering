using Pulumi;
using Pulumi.AzureAD;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Domains;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.HumanIdentities;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

internal class UserBuilder(string resourceName, 
    UserIdentityType userIdentityType, CustomResourceOptions? options = null)
{
    public User Build() =>
        new(resourceName, new()
        {
            UserPrincipalName = $"{userIdentityType.Name}@{DomainType.Default.Name}",
            DisplayName = userIdentityType.DisplayName,
            Password = new PulumiConfig().UserInitialPassword,
            ForcePasswordChange = true
        }, options);
}
