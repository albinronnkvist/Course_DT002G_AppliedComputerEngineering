using Pulumi;

namespace TOrgName.TPlatformPulumiRepositoryName.Core.Helpers;

public static class StackReferenceHelper
{
    public static StackReference GetStackReference(string organization, string projectName, string stackName)
    {
        ValidateInput(organization, projectName, stackName);

        var stack = GetStackName(organization, projectName, stackName);
        
        return new StackReference(stack);
    }
    
    private static string GetStackName(string organization, string projectName, string stackName) => 
        $"{organization}/{projectName}/{stackName}";

    private static void ValidateInput(string organization, string projectName, string stackName)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(organization))
        {
            errors.Add("Organization field is required.");
        }
        
        if (string.IsNullOrWhiteSpace(projectName))
        {
            errors.Add("Project field is required.");
        }
        
        if (string.IsNullOrWhiteSpace(stackName))
        {
            errors.Add("Stack field is required.");
        }

        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(Environment.NewLine, errors));
        }
    }
}
