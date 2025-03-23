using Pulumi;
using Pulumi.Command.Local;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Helpers.Builders;

// azuread.ApplicationFederatedIdentityCredential does not support Claims matching expression (it's a preview feature).
// As a temporary workaround it's achieved with a Command resource.
// Read more: https://www.pulumi.com/registry/packages/azuread/api-docs/applicationfederatedidentitycredential/#constructor-example
internal class FlexibleFederatedIdentityCredentialGitLabCommandBuilder(string resourceName, 
    string name, string repositoryName, Output<string> applicationObjectId, CustomResourceOptions? options = null)
{
    private string _groupName = "TOrgGitLabPath";

    public FlexibleFederatedIdentityCredentialGitLabCommandBuilder WithGroupName(string groupName)
    {
        _groupName = groupName;
        return this;
    }
    
    public Command Build()
    {
        var config = new PulumiConfig();
        var initializerConfig = new Config();
        
        // Hack to detect login when running initializer, due to no easy way found: https://github.com/Azure/azure-cli/issues/6802#issuecomment-506344786
        bool isInitializer = initializerConfig.GetBoolean("isInitializer") ?? false;
        
        var args = new CommandArgs
        {
            Create = applicationObjectId.Apply(appObjectId =>
            {
                string baseCommand = $$"""
                       az rest --method PATCH \
                           --url "https://graph.microsoft.com/beta/applications/{{appObjectId}}/federatedIdentityCredentials(name='{{name}}')" \
                           --headers 'Content-Type=application/json' \
                           --headers 'Prefer=create-if-missing' \
                           --body "{
                               'issuer': 'https://gitlab.com',
                               'audiences': ['api://AzureADTokenExchange'],
                               'claimsMatchingExpression': {
                                   'value': 'claims[\\'sub\\'] matches \\'project_path:{{_groupName}}/{{repositoryName}}:ref_type:branch:ref:*\\'',
                                   'languageVersion': 1
                               }
                           }"
                   """;
            
                if (isInitializer)
                {
                    return baseCommand;
                }
                
                string loginCommand = $"az login --service-principal --username $ARM_CLIENT_ID --tenant {config.TenantId} --federated-token $ARM_OIDC_TOKEN --allow-no-subscriptions";
                
                return $"{loginCommand} && {baseCommand}";
            }),
            Delete = applicationObjectId.Apply(appObjectId =>
            {
                string baseCommand = $"""
                        az rest --method DELETE --url "https://graph.microsoft.com/beta/applications/{appObjectId}/federatedIdentityCredentials/{name}"
                   """;        
                
                if (isInitializer)
                {
                    return baseCommand;
                }
                
                string loginCommand = $"az login --service-principal --username $ARM_CLIENT_ID --tenant {config.TenantId} --federated-token $ARM_OIDC_TOKEN --allow-no-subscriptions";
                
                return $"{loginCommand} && {baseCommand}";
            }),
            Interpreter = new List<string> { "/bin/bash", "-c" }
        };
        
        return new Command(resourceName, args, options);
    }
}
