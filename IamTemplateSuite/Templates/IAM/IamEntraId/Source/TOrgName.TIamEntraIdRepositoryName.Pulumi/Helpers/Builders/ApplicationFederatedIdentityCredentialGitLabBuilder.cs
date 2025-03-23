using Pulumi;
using Pulumi.AzureAD;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

internal class ApplicationFederatedIdentityCredentialGitLabBuilder(string resourceName, 
    string name, string repositoryName, Output<string> applicationId, CustomResourceOptions? options = null)
{
    private string _groupName = "TOrgGitLabPath";

    public ApplicationFederatedIdentityCredentialGitLabBuilder WithGroupName(string groupName)
    {
        _groupName = groupName;
        return this;
    }
    
    public ApplicationFederatedIdentityCredential Build()
    {
        return new ApplicationFederatedIdentityCredential(resourceName, new()
        {
            ApplicationId = applicationId,
            #pragma warning disable CA1861
            Audiences = new[] { "api://AzureADTokenExchange" },
            #pragma warning restore CA1861
            DisplayName = name,
            Issuer = "https://gitlab.com",
            Subject = $"project_path:{_groupName}/{repositoryName}:ref_type:branch:ref:main"
        }, options);
    }
}
