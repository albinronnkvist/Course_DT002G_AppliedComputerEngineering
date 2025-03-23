namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;

public static class ResourceNamingConvention
{
    /// <summary>
    /// Creates a resource name based on a naming convention
    /// </summary>
    /// <param name="resourceAbbreviation">An abbreviation that represents the type of resource.</param>
    /// <param name="instance">Instance name for a specific resource, to differentiate it from other resources that have the same naming convention and naming components.</param>
    /// <returns>The full resource name.</returns>
    public static string GetResourceName(string resourceAbbreviation, string instance) => 
        $"{resourceAbbreviation}-{instance}";
    
    /// <summary>
    /// Creates a resource name based on the abbreviation of an <see cref="IResourceType"/> and a naming convention
    /// </summary>
    /// <param name="resourceType">An <see cref="IResourceType"/> that represents the type of resource.</param>
    /// <param name="instance">Instance name for a specific resource, to differentiate it from other resources that have the same naming convention and naming components.</param>
    /// <returns>The full resource name.</returns>
    public static string GetResourceName(IResourceType resourceType, string instance) => 
        GetResourceName(resourceType.Abbreviation, instance);
}
