using TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Mappers;

internal static class ApplicationIdentityTypeContractMapper
{
    private static readonly Dictionary<ApplicationIdentityType, Contract.Enums.ApplicationIdentityType> Mappings = new()
    {
        { ApplicationIdentityType.TIamEntraIdRepositoryName, Contract.Enums.ApplicationIdentityType.TIamEntraIdRepositoryName },
        { ApplicationIdentityType.TIamAzureRepositoryName, Contract.Enums.ApplicationIdentityType.TIamAzureRepositoryName },
        { ApplicationIdentityType.TContainerizationRepositoryName, Contract.Enums.ApplicationIdentityType.TContainerizationRepositoryName },
        { ApplicationIdentityType.TMicroserviceRepositoryName, Contract.Enums.ApplicationIdentityType.TMicroserviceRepositoryName }
    };

    public static Contract.Enums.ApplicationIdentityType Map(ApplicationIdentityType applicationIdentityType)
    {
        if (Mappings.TryGetValue(applicationIdentityType, out Contract.Enums.ApplicationIdentityType repositoryType))
        {
            return repositoryType;
        }

        throw new KeyNotFoundException($"No RepositoryType mapping found for ApplicationIdentityType '{applicationIdentityType.Name}'");
    }
}
