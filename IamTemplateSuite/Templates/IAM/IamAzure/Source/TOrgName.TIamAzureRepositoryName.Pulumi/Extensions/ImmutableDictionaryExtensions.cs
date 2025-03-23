using System.Collections.Immutable;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Extensions;

internal static class ImmutableDictionaryExtensions
{
    public static TValue GetValueOrThrow<TKey, TValue>(this ImmutableDictionary<TKey, TValue> dictionary, 
        TKey key) where TKey : notnull
    {
        if (!dictionary.TryGetValue(key, out TValue? value))
        {
            throw new KeyNotFoundException($"Key '{key}' was not found or has an invalid value.");
        }

        ArgumentNullException.ThrowIfNull(value);

        return value;
    }
}
