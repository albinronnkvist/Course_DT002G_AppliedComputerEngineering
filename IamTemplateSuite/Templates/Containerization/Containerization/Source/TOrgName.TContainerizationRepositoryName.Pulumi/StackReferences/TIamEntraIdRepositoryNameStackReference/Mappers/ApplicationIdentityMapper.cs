using System.Collections.Immutable;
using System.Text.Json;

namespace TOrgName.TContainerizationRepositoryName.Pulumi.StackReferences.TIamEntraIdRepositoryNameStackReference.Mappers;

internal static class ApplicationIdentityMapper
{
    private static readonly Dictionary<TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.Enums.ApplicationIdentityType, ApplicationIdentityType> Mappings = new()
    {
        [TIamEntraIdRepositoryName.Pulumi.Contract.Enums.ApplicationIdentityType.TIamEntraIdRepositoryName] = ApplicationIdentityType.InfrastructureIamEntraid,
        [TIamEntraIdRepositoryName.Pulumi.Contract.Enums.ApplicationIdentityType.TIamAzureRepositoryName] = ApplicationIdentityType.InfrastructureIamAzurerbac,
        [TIamEntraIdRepositoryName.Pulumi.Contract.Enums.ApplicationIdentityType.TContainerizationRepositoryName] = ApplicationIdentityType.InfrastructureContainerization,
        [TIamEntraIdRepositoryName.Pulumi.Contract.Enums.ApplicationIdentityType.TMicroserviceRepositoryName] = ApplicationIdentityType.ExampleMicroservice,
    };
    
    internal static ImmutableDictionary<ApplicationIdentityType, ApplicationIdentity> Map(ImmutableDictionary<string, object> applicationIdentities) =>
        applicationIdentities.ToImmutableDictionary(
            kvp => MapApplicationIdentityType(ParseApplicationIdentityType(kvp.Key)),
            kvp => MapApplicationIdentity(ParseApplicationIdentity(kvp))
        );

    private static TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.Enums.ApplicationIdentityType ParseApplicationIdentityType(string typeString) =>
        Enum.Parse<TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.Enums.ApplicationIdentityType>(typeString);

    private static TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes.ApplicationIdentity ParseApplicationIdentity(KeyValuePair<string, object> kvp)
    {
        if (kvp.Value is not string json)
        {
            throw new InvalidOperationException($"Expected string value for ApplicationIdentityType key: {kvp.Key}");
        }

        return JsonSerializer.Deserialize<TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes.ApplicationIdentity>(json)
               ?? throw new InvalidOperationException($"Failed to deserialize ApplicationIdentityType: {kvp.Key}");
    }
    
    private static ApplicationIdentityType MapApplicationIdentityType(TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.Enums.ApplicationIdentityType contractApplicationIdentityType) => 
        Mappings.TryGetValue(contractApplicationIdentityType, out var applicationIdentityType) 
            ? applicationIdentityType 
            : throw new KeyNotFoundException($"No contract mapping found for ApplicationIdentityType '{contractApplicationIdentityType.ToString()}'.");
    
    private static ApplicationIdentity MapApplicationIdentity(TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes.ApplicationIdentity applicationIdentity) => 
        new(applicationIdentity.ApplicationClientId, applicationIdentity.ServicePrincipalObjectId);
}

