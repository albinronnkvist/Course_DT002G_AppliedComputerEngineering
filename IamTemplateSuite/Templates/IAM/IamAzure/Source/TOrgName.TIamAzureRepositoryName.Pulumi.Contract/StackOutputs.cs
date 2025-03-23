using System.Collections.Immutable;
using TOrgName.TIamAzureRepositoryName.Pulumi.Contract.Enums;
using TOrgName.TIamAzureRepositoryName.Pulumi.Contract.OutputTypes;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Contract;

public static class StackOutputs
{
    /// <summary>
    /// An <see cref="ImmutableDictionary{TKey,TValue}"/> where:
    /// <list type="bullet">
    ///   <item>
    ///     <term>TKey: </term>
    ///     <description><see cref="ResourceGroupType"/> converted to a <see cref="string"/>.</description>
    ///   </item>
    ///   <item>
    ///     <term>TValue: </term>
    ///     <description>A JSON Serialized <see cref="ResourceGroup"/>.</description>
    ///   </item>
    /// </list>
    /// </summary>
    public const string ResourceGroups = "resourceGroups";
}
