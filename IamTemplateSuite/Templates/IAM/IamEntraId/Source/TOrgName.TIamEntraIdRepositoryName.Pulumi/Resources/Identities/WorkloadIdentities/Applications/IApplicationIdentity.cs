using Pulumi;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Resources.Identities.WorkloadIdentities.Applications;

internal interface IApplicationIdentity
{
    public Output<string> ApplicationClientId { get; }
    public Output<string> ServicePrincipalObjectId { get; }
}
