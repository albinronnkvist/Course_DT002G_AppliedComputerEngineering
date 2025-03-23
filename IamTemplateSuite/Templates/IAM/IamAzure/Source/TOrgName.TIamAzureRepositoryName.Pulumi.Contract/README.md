# Contract

## Overview

This package standardizes stack references and outputs for IAM-related resources provisioned in the TIamAzureRepositoryName project. It ensures consistency and type safety across stacks.

### Features

- **Stack References:** Predefined constants for organization, project, and stack names.
- **Stack Outputs:** Named outputs with typed values for structured integration.

## Usage

Reference the stack, get output details and parse and/or deserialize them.
Pulumi stack outputs in .NET do not support strongly typed values. Instead, stack outputs must be retrieved as dictionaries or primitive types, and then manually parsed or deserialized as needed.

### Example

1. Retrieve stack outputs using Pulumi stack references:

    ```csharp
    var stackReference = StackReferenceHelper.GetStackReference(
                StackDetails.Organization,
                StackDetails.Project, 
                StackNames.Dev);
    ```

2. Get output details from a specific stack output (`StackOutputs.*`):

    ```csharp
    var outputDetails = await stackReference
        .GetOutputDetailsAsync(StackOutputs.ResourceGroups);
            
        if (outputDetails.Value is not ImmutableDictionary<string, object> resourceGroups)
        {
            throw new InvalidOperationException($"Stack reference output '{StackOutputs.ResourceGroups}' was not a valid type.");
        }
    ```

3. Get the value from an output and parse and/or deserialize it (`Enums*` and `Outputs*`):

    ```csharp
    if (resourceGroups.TryGetValue(Enums.ResourceGroupType.TMicroserviceRepositoryName.ToString(), out var json))
    {
        var resourceGroup = JsonSerializer.Deserialize<Outputs.ResourceGroup>(json);
    }
    ```



