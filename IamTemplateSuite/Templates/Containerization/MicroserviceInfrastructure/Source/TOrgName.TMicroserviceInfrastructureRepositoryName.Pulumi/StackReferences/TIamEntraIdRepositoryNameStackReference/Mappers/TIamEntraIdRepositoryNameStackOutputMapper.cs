using System.Collections.Immutable;
using System.Text.Json;
using Pulumi;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.Enums;

namespace TOrgName.TMicroserviceInfrastructureRepositoryName.Pulumi.StackReferences.TIamEntraIdRepositoryNameStackReference.Mappers;

internal static class TIamEntraIdRepositoryNameStackOutputMapper
{
    internal static async Task<ApplicationIdentity> GetApplicationIdentityAsync(StackReference stackReference)
    {
        var outputDetails = await stackReference
            .GetOutputDetailsAsync(StackOutputs.ApplicationIdentities);
        
        if (outputDetails.Value is not ImmutableDictionary<string, object> applicationIdentities)
        {
            throw new InvalidOperationException($"Stack reference output '{StackOutputs.ApplicationIdentities}' was not a valid type.");
        }

        if (!applicationIdentities.TryGetValue(
                ApplicationIdentityType.TMicroserviceRepositoryName.ToString(),
                out var exampleMicroserviceApplicationIdentity))
        {
            throw new KeyNotFoundException($"ApplicationIdentityType '{ApplicationIdentityType.TMicroserviceRepositoryName}' key not found.");
        }
        
        return MapApplicationIdentity(ParseApplicationIdentity(exampleMicroserviceApplicationIdentity));
    }
    
    private static ApplicationIdentity MapApplicationIdentity(TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes.ApplicationIdentity applicationIdentity) => 
        new(applicationIdentity.ApplicationClientId, applicationIdentity.ServicePrincipalObjectId);

    private static TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes.ApplicationIdentity ParseApplicationIdentity(object applicationIdentity)
    {
        if (applicationIdentity is not string json)
        {
            throw new InvalidOperationException("Expected string value");
        }

        return JsonSerializer.Deserialize<TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes.ApplicationIdentity>(json)
               ?? throw new InvalidOperationException("Failed to deserialize ApplicationIdentity");
    }
}
