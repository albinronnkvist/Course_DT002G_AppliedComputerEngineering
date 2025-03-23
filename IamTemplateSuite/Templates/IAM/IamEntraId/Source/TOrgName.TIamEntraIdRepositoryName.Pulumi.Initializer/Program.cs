using TOrgName.TIamEntraIdRepositoryName.Pulumi.Initializer;

#pragma warning disable CA1303
if (args.Length < 3)
{
    Console.WriteLine("❌ Error: Missing required arguments. Usage: dotnet run <stackName> <tenantId> <userInitialPassword> -c Release");
    return;
}

string stackName = args[0];
string tenantId = args[1];
string userInitialPassword = args[2];

Console.WriteLine($"🚀 Setting up {stackName} stack with Tenant ID: {tenantId}, and  User Initial-Password: [HIDDEN]");

await StackInitializer.Initialize(stackName, new StackConfiguration(tenantId, userInitialPassword));
#pragma warning restore CA1303
