using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Identities.WorkloadIdentities;
using Pulumi.Automation;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Initializer;

#pragma warning disable CA1303
internal static class StackInitializer
{
    private const string PulumiProjectName = "TOrgName.TIamAzureRepositoryName.Pulumi";
    
    public static async Task Initialize(string stackName, StackConfiguration stackConfiguration)
    {
        try
        {
            (WorkspaceStack stack, Workspace workspace) = await SetupWorkspaceStackAsync(stackName);

            await SetPreStackConfigurationAsync(stack, stackConfiguration);

            Console.WriteLine("Running pulumi refresh...");
            await stack.RefreshAsync(new RefreshOptions { OnStandardOutput = Console.WriteLine });
            
            Console.WriteLine("Running pulumi up...");
            await stack.UpAsync(new UpOptions { OnStandardOutput = Console.WriteLine });
            
            await SetPostStackConfigurationAsync(stack);

            Console.WriteLine("✅ Process complete! Refer to the post-deployment instructions.");
            
            await PrintPostInstructions(workspace);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing Pulumi stack: {ex.Message}");
            throw;
        }
    }

    private static async Task<(WorkspaceStack, Workspace)> SetupWorkspaceStackAsync(string stackName)
    {
        Console.WriteLine("Creating workspace...");
        
        string? executingDir = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent?.FullName;
        ArgumentException.ThrowIfNullOrWhiteSpace(executingDir);
        string workingDir = Path.Combine(executingDir, "..", "..", "..", "..", PulumiProjectName);
            
        using LocalWorkspace workspace = await LocalWorkspace.CreateAsync(new LocalWorkspaceOptions
        {
            WorkDir = workingDir
        });
            
        WorkspaceStack stack = await LocalWorkspace.CreateOrSelectStackAsync(
            new LocalProgramArgs(stackName, workingDir));
        
        Console.WriteLine("Workspace was successfully created.");
        
        return (stack, workspace);
    }
    
    private static async Task SetPreStackConfigurationAsync(WorkspaceStack stack, StackConfiguration stackConfiguration)
    {
        await stack.SetConfigAsync("isInitializer", new ConfigValue("true"));
        await stack.SetConfigAsync("azure-native:tenantId", new ConfigValue(stackConfiguration.TenantId));
        await stack.SetConfigAsync("azure-native:subscriptionId", new ConfigValue(stackConfiguration.SubscriptionId));
        await stack.SetConfigAsync("azure-native:location", new ConfigValue(stackConfiguration.Location));
    }

    private static async Task SetPostStackConfigurationAsync(WorkspaceStack stack)
    {
        Console.WriteLine("Updating post pulumi configuration...");

        await stack.RemoveConfigAsync("isInitializer");
        
        Console.WriteLine("Post pulumi configuration was successfully updated.");
    }
    
    private static async Task PrintPostInstructions(Workspace workspace)
    {
        Console.WriteLine("Determining post-deployment instructions...");
        
        string iamEntraidStackName = $"{TIamEntraIdRepositoryName.Pulumi.Contract.StackDetails.Organization}/{TIamEntraIdRepositoryName.Pulumi.Contract.StackDetails.Project}/{TIamEntraIdRepositoryName.Pulumi.Contract.StackNames.Main}";
        WorkspaceStack iamEntraidMainStack = await WorkspaceStack.SelectAsync(iamEntraidStackName, workspace);

        ImmutableDictionary<string, OutputValue> outputs = await iamEntraidMainStack.GetOutputsAsync();
        
        if (!outputs.TryGetValue(TIamEntraIdRepositoryName.Pulumi.Contract.StackOutputs.ApplicationIdentities, out OutputValue? applicationIdentitiesOutput) 
            || applicationIdentitiesOutput.Value is not Dictionary<string, object> applicationIdentities)
        {
            throw new InvalidOperationException("Application identities output not found.");
        }
        
        if (!applicationIdentities.TryGetValue(TIamEntraIdRepositoryName.Pulumi.Contract.Enums.ApplicationIdentityType.TIamAzureRepositoryName.ToString(), out object? applicationIdentity) 
            || applicationIdentity is not string json)
        {
            throw new InvalidOperationException("Invalid or missing ApplicationIdentity.");
        }
        
        ApplicationIdentity deserializedApplicationIdentity = JsonSerializer.Deserialize<ApplicationIdentity>(json) 
                                                              ?? throw new InvalidOperationException("Failed to deserialize ApplicationIdentity.");
        
        Console.WriteLine("Manual post-deployment steps:");
        Console.WriteLine($"1. Update `.gitlab-ci.yml` with the following ARM_CLIENT_ID: {deserializedApplicationIdentity.ApplicationClientId}");
    }
}
#pragma warning restore CA1303

