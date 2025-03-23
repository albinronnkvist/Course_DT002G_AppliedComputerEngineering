using System.Reflection;
using System.Text.Json;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.Enums;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes;
using Pulumi.Automation;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Initializer;

#pragma warning disable CA1303
internal static class StackInitializer
{
    private const string PulumiProjectName = "TOrgName.TIamEntraIdRepositoryName.Pulumi";
    
    public static async Task Initialize(string stackName, StackConfiguration stackConfiguration)
    {
        try
        {
            (bool stackExists, WorkspaceStack stack) = await SetupWorkspaceStackAsync(stackName);

            await SetPreStackConfigurationAsync(stack, stackExists, stackConfiguration);

            Console.WriteLine("Running pulumi refresh...");
            await stack.RefreshAsync(new RefreshOptions { OnStandardOutput = Console.WriteLine });
            
            Console.WriteLine("Running pulumi up...");
            UpResult result = await stack.UpAsync(new UpOptions { OnStandardOutput = Console.WriteLine });
            
            await SetPostStackConfigurationAsync(stack);
            
            Console.WriteLine("✅ Process complete! Refer to the post-deployment instructions.");
            
            PrintPostInstructions(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing Pulumi stack: {ex.Message}");
            throw;
        }
    }

    private static async Task<(bool stackExists, WorkspaceStack stack)> SetupWorkspaceStackAsync(string stackName)
    {
        Console.WriteLine("Creating workspace...");
        
        string? executingDir = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent?.FullName;
        ArgumentException.ThrowIfNullOrWhiteSpace(executingDir);
        string workingDir = Path.Combine(executingDir, "..", "..", "..", "..", PulumiProjectName);
            
        using LocalWorkspace workspace = await LocalWorkspace.CreateAsync(new LocalWorkspaceOptions
        {
            WorkDir = workingDir
        });

        bool stackExists = (await workspace.ListStacksAsync())
            .Any(s => s.Name.Equals(stackName, StringComparison.Ordinal));
            
        WorkspaceStack stack = await LocalWorkspace.CreateOrSelectStackAsync(
            new LocalProgramArgs(stackName, workingDir));
        
        Console.WriteLine("Workspace was successfully created.");
        
        return (stackExists, stack);
    }
    
    private static async Task SetPreStackConfigurationAsync(WorkspaceStack stack, bool stackExists, StackConfiguration stackConfiguration)
    {
        await stack.SetConfigAsync("isInitializer", new ConfigValue("true"));
        await stack.SetConfigAsync("azuread:tenantId", new ConfigValue(stackConfiguration.TenantId));
        if (!stackExists)
        {
            await stack.SetConfigAsync("userInitialPassword", new ConfigValue(stackConfiguration.UserInitialPassword, true));
        }
    }

    private static async Task SetPostStackConfigurationAsync(WorkspaceStack stack)
    {
        Console.WriteLine("Updating post pulumi configuration...");

        await stack.RemoveConfigAsync("isInitializer");
        
        Console.WriteLine("Post pulumi configuration was successfully updated.");
    }
    
    private static void PrintPostInstructions(UpResult result)
    {
        Console.WriteLine("Determining post-deployment instructions...");
        
        if (!result.Outputs.TryGetValue(StackOutputs.ApplicationIdentities, out OutputValue? applicationIdentitiesOutput) 
            || applicationIdentitiesOutput.Value is not Dictionary<string, object> applicationIdentities)
        {
            throw new InvalidOperationException("Application identities output not found.");
        }

        if (!applicationIdentities.TryGetValue(ApplicationIdentityType.TIamEntraIdRepositoryName.ToString(), out object? applicationIdentity) 
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

