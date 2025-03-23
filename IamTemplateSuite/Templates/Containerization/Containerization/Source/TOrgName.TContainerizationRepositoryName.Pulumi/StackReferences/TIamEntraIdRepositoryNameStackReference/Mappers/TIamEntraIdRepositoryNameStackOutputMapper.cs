using System.Collections.Immutable;
using Pulumi;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract;

namespace TOrgName.TContainerizationRepositoryName.Pulumi.StackReferences.TIamEntraIdRepositoryNameStackReference.Mappers;

internal static class TIamEntraIdRepositoryNameStackOutputMapper
{
    internal static async Task<ImmutableDictionary<ApplicationIdentityType, ApplicationIdentity>> GetApplicationIdentitiesAsync(StackReference stackReference)
    {
        var outputDetails = await stackReference
            .GetOutputDetailsAsync(StackOutputs.ApplicationIdentities);
        
        if (outputDetails.Value is not ImmutableDictionary<string, object> applicationIdentities)
        {
            throw new InvalidOperationException($"Stack reference output '{StackOutputs.ApplicationIdentities}' was not a valid type.");
        }

        return ApplicationIdentityMapper.Map(applicationIdentities);
    }
}
