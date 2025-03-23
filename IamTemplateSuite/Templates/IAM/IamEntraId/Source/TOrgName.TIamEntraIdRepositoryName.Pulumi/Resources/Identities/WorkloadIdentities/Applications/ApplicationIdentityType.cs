using Ardalis.SmartEnum;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications;

internal sealed class ApplicationIdentityType : SmartEnum<ApplicationIdentityType>
{
    public static readonly ApplicationIdentityType TIamEntraIdRepositoryName = new("TIamEntraIdRepositoryGitLabPath", 1, "TIamEntraIdRepositoryName", "TIamEntraIdRepositoryGitLabPath");
   
    public static readonly ApplicationIdentityType TIamAzureRepositoryName = new("TIamAzureRepositoryGitLabPath", 2, "TIamAzureRepositoryName", "TIamAzureRepositoryGitLabPath");
    
    public static readonly ApplicationIdentityType TContainerizationRepositoryName = new("TContainerizationRepositoryGitLabPath", 3, "TContainerizationRepositoryName", "TContainerizationRepositoryGitLabPath");
    
    public static readonly ApplicationIdentityType TMicroserviceRepositoryName = new("TMicroserviceRepositoryGitLabPath", 4, "TMicroserviceRepositoryName", "TMicroserviceRepositoryGitLabPath");
    
    public string DisplayName { get; }
    public string RepositoryPath { get; }

    private ApplicationIdentityType(string name, int value,
        string displayName, string repositoryPath) : base(name, value)
    {
        DisplayName = displayName;
        RepositoryPath = repositoryPath;
    }
}
