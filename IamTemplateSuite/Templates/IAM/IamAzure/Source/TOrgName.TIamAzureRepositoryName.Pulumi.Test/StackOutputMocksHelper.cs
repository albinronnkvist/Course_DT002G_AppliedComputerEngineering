using System.Collections.Immutable;
using System.Text.Json;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.Enums;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Test;

internal static class StackOutputMocksHelper
{
    public static object GetTIamEntraIdRepositoryNameStackOutput(string stackOutputName) =>
        stackOutputName switch
        {
            _ when stackOutputName.Equals(TIamEntraIdRepositoryName.Pulumi.Contract.StackOutputs.ApplicationIdentities, 
                StringComparison.Ordinal) => ApplicationIdentitiesStackOutputs,
            _ => throw new InvalidOperationException()
        };
    
    private static readonly ImmutableDictionary<string, object> ApplicationIdentitiesStackOutputs = new Dictionary<string, object>
    {
        {
            ApplicationIdentityType.TIamEntraIdRepositoryName.ToString(),
            JsonSerializer.Serialize(new ApplicationIdentity("136c7311-613a-4eee-9dc7-6ffbb02929af",
                "14fa054f-63c1-49b0-b168-ed422d6f99f5"))
        },
        {
            ApplicationIdentityType.TIamAzureRepositoryName.ToString(),
            JsonSerializer.Serialize(new ApplicationIdentity("736c7311-613a-4eee-9dc7-6ffbb02929af",
                "94fa054f-63c1-49b0-b168-ed422d6f99f5"))
        },
        {
            ApplicationIdentityType.TContainerizationRepositoryName.ToString(),
            JsonSerializer.Serialize(new ApplicationIdentity("4bbe5fbe-55cd-42f3-a487-71de83e57b06",
                "48252586-836d-44e8-9281-ed28b8feb1c6"))
        },
        {
            ApplicationIdentityType.TMicroserviceRepositoryName.ToString(),
            JsonSerializer.Serialize(new ApplicationIdentity("4ed3889d-a38a-4069-be82-82fe524bc2b5",
                "4c239f45-8a11-4b28-96f1-a71fbdbb185b"))
        }
    }.ToImmutableDictionary();
}
