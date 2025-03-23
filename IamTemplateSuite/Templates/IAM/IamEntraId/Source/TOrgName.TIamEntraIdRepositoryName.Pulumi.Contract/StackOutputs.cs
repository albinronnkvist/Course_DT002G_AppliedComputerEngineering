using System.Collections.Immutable;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.Enums;
using TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.OutputTypes;

namespace TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract;

public static class StackOutputs
{
    /// <summary>
    /// An <see cref="ImmutableDictionary{TKey, TValue}"/> where:
    /// <list type="bullet">
    ///   <item>
    ///     <term>TKey: </term>
    ///     <description><see cref="ApplicationIdentityType"/> converted to a <see cref="string"/>.</description>
    ///   </item>
    ///   <item>
    ///     <term>TValue: </term>
    ///     <description>A JSON Serialized <see cref="ApplicationIdentity"/>.</description>
    ///   </item>
    /// </list>
    /// </summary>
    public const string ApplicationIdentities = "applicationIdentities";
}
