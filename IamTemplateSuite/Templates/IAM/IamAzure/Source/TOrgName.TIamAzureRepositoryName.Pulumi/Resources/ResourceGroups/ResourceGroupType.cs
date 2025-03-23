using Ardalis.SmartEnum;

namespace TOrgName.TIamAzureRepositoryName.Pulumi.Resources.ResourceGroups;

internal sealed class ResourceGroupType : SmartEnum<ResourceGroupType>
{
    public static readonly ResourceGroupType TContainerizationRepositoryName = new("TContainerizationRepositoryGitLabPath", 1, "TContainerizationRepositoryGitLabPath");
    public static readonly ResourceGroupType TMicroserviceRepositoryName = new("TMicroserviceRepositoryGitLabPath", 2, "TMicroserviceRepositoryGitLabPath");
    
    public string WorkloadName { get; }

    private ResourceGroupType(string name, int value, string workloadName) : base(name, value) => 
        WorkloadName = workloadName;
}
