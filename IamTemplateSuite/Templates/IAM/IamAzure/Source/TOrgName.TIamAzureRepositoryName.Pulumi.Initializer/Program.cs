using TOrgName.TIamAzureRepositoryName.Pulumi.Initializer;

#pragma warning disable CA1303
if (args.Length < 4)
{
    Console.WriteLine("❌ Error: Missing required arguments. Usage: dotnet run <stackName> <tenantId> <subscriptionId> <location> -c Release");
    return;
}

string stackName = args[0];
string tenantId = args[1];
string subscriptionId = args[2];
string location = args[3];

Console.WriteLine($"🚀 Setting up {stackName} stack with Tenant ID: {tenantId}, Subscription ID: {subscriptionId}, and location: {location}");

await StackInitializer.Initialize(stackName, new StackConfiguration(tenantId, subscriptionId, location));
#pragma warning restore CA1303

