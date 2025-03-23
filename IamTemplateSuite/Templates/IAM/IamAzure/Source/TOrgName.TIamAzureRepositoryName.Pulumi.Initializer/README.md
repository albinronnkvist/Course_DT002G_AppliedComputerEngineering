# Pulumi Initializer

This project initializes the Pulumi project, creating the stacks and provisioning Azure IAM resources from scratch.
The program can also be run with an existing stack without destroying it, making it safe to execute multiple times.

## Overview

Running this program will:

- Create the specified Pulumi stack.
- Provision all required Azure IAM resources as defined in the Pulumi project, including:
    - Self-assigning an Azure Owner role for the specified subscription.
- Update Pulumi configuration files (`Pulumi*.yaml`) with the provided settings.
- Display post-deployment instructions for any manual steps required.

Once the execution is complete, follow the post-deployment instructions further down in this document.

## Pre-requisites

- **Azure Authentication:** Sign in as a _Global Administrator_ using `az login`
- **Pulumi Authentication:** Sign in to Pulumi using `pulumi login`
- **A New Git Branch:** Since this program modifies the Pulumi project's code, create a new branch before running it.

## Usage

Run the following command:

```sh
dotnet run <stackName> <tenantId> <subscriptionId> <location> -c Release
```

- `<stackName>`: The name of the Pulumi stack (e.g., dev, prod).
- `<tenantId>`: Your Azure tenant ID.
- `<subscriptionId>`: The Azure Subscription ID where resources will be created.
- `<location>`: The Azure region (e.g., eastus, westeurope).

Ensure you run the initializer in Release mode, as the binary path is set in Pulumi.yaml:
```markdown
runtime:
  name: dotnet
  options:
    binary: bin/Release/net9.0/TOrgName.TIamAzureRepositoryName.Pulumi.dll
```

### Example

```sh
dotnet run "dev" "ac03e6c4-4885-4ca2-a6a4-e9dfca42d88a" "679ab7ed-b25a-43c9-8bb9-f026a4c7bc47" "swedencentral" -c Release
```

## Post-Deployment Instructions

After running the program, follow these manual post-deployment steps to complete the setup:

1. **Update GitLab CI/CD Configuration**
    - Modify your `.gitlab-ci.yml` file to include the `ARM_CLIENT_ID` printed in the console after deployment. 
    - Example: 
      ```sh
      variables:
        ARM_CLIENT_ID: "9761ce9f-cce0-4b77-a7f6-00f72340e063"
      ```
    
2. **Commit the Changes**
    - The program makes modifications to the Pulumi project. Ensure all changes are committed. 

3. **Merge Changes & Trigger CI/CD Deployment**
    - Push the changes and create a merge request.
    - Merge the changes into the main branch and allow the CI/CD pipeline to handle the deployment.

Once the CI/CD pipeline successfully completes, your Pulumi stack will be fully deployed! 🚀
Future deployments will leverage federated authentication, eliminating the need for a Global Administrator.

