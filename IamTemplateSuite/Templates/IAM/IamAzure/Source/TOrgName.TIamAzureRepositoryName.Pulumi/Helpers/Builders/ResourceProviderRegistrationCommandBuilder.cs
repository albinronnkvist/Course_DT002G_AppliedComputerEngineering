using Pulumi;
using Pulumi.Command.Local;
using TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceProviders;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Builders;

internal sealed class ResourceProviderRegistrationCommandBuilder(string resourceName, 
    ResourceProviderType resourceProviderType, CustomResourceOptions? customResourceOptions = null)
{
    public Command Build() =>
        new(resourceName, new CommandArgs
        {
            Create = GetFullCommand(ProviderAction.Register),
            Delete = GetFullCommand(ProviderAction.Unregister),
            Interpreter = new List<string> { "/bin/bash", "-c" }
        }, customResourceOptions);

    private string GetFullCommand(ProviderAction action)
    {
        string baseCommand = GetBaseCommand(action);

        // Hack to detect login when running initializer, due to no easy way found: https://github.com/Azure/azure-cli/issues/6802#issuecomment-506344786
        bool isInitializer = new Config().GetBoolean("isInitializer") ?? false;
        if (isInitializer)
        {
            return baseCommand;
        }

        var config = new PulumiConfig();
        string loginCommand = $"az login --service-principal --username $ARM_CLIENT_ID --tenant {config.TenantId} --federated-token $ARM_OIDC_TOKEN --allow-no-subscriptions";

        return $"{loginCommand} && {baseCommand}";
    }
    
    private string GetBaseCommand(ProviderAction action) => action switch
    {
        ProviderAction.Register => $"az provider register --namespace {resourceProviderType.Namespace}",
        ProviderAction.Unregister => $"az provider unregister --namespace {resourceProviderType.Namespace}",
        _ => throw new ArgumentOutOfRangeException(nameof(action), "Unsupported provider action")
    };
    
    private enum ProviderAction
    {
        Register = 1,
        Unregister = 2
    }
}
