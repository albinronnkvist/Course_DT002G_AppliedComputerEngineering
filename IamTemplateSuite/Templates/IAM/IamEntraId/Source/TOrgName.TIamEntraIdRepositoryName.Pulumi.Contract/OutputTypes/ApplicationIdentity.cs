namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes;

/// <summary>
/// An Application with an ID and a Principal ID
/// </summary>
/// <param name="ApplicationClientId"> The Application Client ID</param>
/// <param name="ServicePrincipalObjectId"> The Application's Service Principal Object ID</param>
public record ApplicationIdentity(string ApplicationClientId, string ServicePrincipalObjectId);
