using Shouldly;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes;
using TOrgName.TPlatformPulumiAzureRepositoryName.Core.ResourceTypes.Types;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.Test.ResourceTypesTests;

public class ResourceNamingConventionTests
{
    [Fact]
    public void GetResourceNameWithValidAbbreviationAndInstanceReturnsCorrectFormat()
    {
        const string abbreviation = "vm";
        const string instance = "microservice";

        ResourceNamingConvention.GetResourceName(abbreviation, instance)
            .ShouldBe($"{abbreviation}-{instance}");
    }

    [Fact]
    public void GetResourceNameWithValidResourceTypeAndInstanceReturnsCorrectFormat()
    {
        var resourceType = AzureResourceType.KeyVault;
        const string instance = "test";
        
        ResourceNamingConvention.GetResourceName(resourceType, instance)
            .ShouldBe($"{resourceType.Abbreviation}-{instance}");
    }
}
