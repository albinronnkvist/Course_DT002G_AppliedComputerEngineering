using System.Collections.Immutable;
using Pulumi;
using TOrgName.TIamAzureRepositoryName.Pulumi.Helpers.Identities.WorkloadIdentities;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.StackReferences.TIamEntraIdRepositoryNameStackReference.Mappers;

internal static class TIamEntraIdRepositoryNameStackOutputMapper
{
    internal static async Task<ImmutableDictionary<ApplicationIdentityType, ApplicationIdentity>> GetApplicationIdentitiesAsync(StackReference stackReference)
    {
        StackReferenceOutputDetails outputDetails = await stackReference
            .GetOutputDetailsAsync(TIamEntraIdRepositoryName.Pulumi.Contract.StackOutputs.ApplicationIdentities);
        
        if (outputDetails.Value is not ImmutableDictionary<string, object> applicationIdentities)
        {
            throw new InvalidOperationException($"Stack reference output '{TIamEntraIdRepositoryName.Pulumi.Contract.StackOutputs.ApplicationIdentities}' was not a valid type.");
        }

        return ApplicationIdentityMapper.Map(applicationIdentities);
    }
}
