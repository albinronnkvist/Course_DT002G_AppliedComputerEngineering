# Pulumi Initializer

This project initializes the Pulumi project, creating the stacks and provisioning Entra ID IAM resources from scratch.
The program can also be run with an existing stack without destroying it, making it safe to execute multiple times.

## Overview

Running this program will:

- Create the specified Pulumi stack.
- Provision all required Entra ID IAM resources as defined in the Pulumi project, including:
  - Self-provision an Application Identity with a Federated Identity Credential (FIC).
- Update Pulumi configuration files (`Pulumi*.yaml`) with the provided settings.
- Display post-deployment instructions for any manual steps required.

Once the execution is complete, follow the post-deployment instructions further down in this document.

## Pre-requisites

- **Azure Authentication:** Sign in as a _Global Administrator_ using `az login`
- **Pulumi Authentication:** Sign in to Pulumi using `pulumi login`
- **A New Git Branch:** Since this program modifies the Pulumi project's code, create a new branch before running it.

**⚠️ [Entra ID Directory Role Issue & Workaround](https://github.com/hashicorp/terraform-provider-azuread/issues/1526):** Pulumi encounters issues when provisioning directory roles because unassigned roles are not exposed via the Microsoft Graph API.
Azure does not consider a directory role "activated" until it has been assigned to at least one user or service principal. Since Pulumi relies on the API to retrieve role details, any unassigned role remains invisible, preventing it from being managed programmatically.
  - **Workaround:** Manually assign the directory roles (see `DirectoryRoleType`) to any user or service principal in the Azure portal, before provisioning it with Pulumi. Once assigned, Azure marks the role as activated, making it detectable via the API and manageable by Pulumi.

## Usage

Run the following command:

```sh
dotnet run <stackName> <tenantId> <userInitialPassword> -c Release
```

- `<stackName>`: The name of the Pulumi stack (e.g., dev, prod).
- `<tenantId>`: Your Azure tenant ID.
- `<userInitialPassword>`: The default password assigned to newly registered users.
  - 🔒 This password is stored securely as a secret in Pulumi configuration.

Ensure you run the initializer in Release mode, as the binary path is set in Pulumi.yaml:
```markdown
runtime:
  name: dotnet
  options:
    binary: bin/Release/net9.0/OrgName.InfrastructureIamEntraid.Pulumi.dll
```

### Example

```sh
dotnet run "main" "b050ab12-5b27-4459-9c54-cd9c888ba38b" "MostSecureP#w192!" -c Release
```

## Post-Deployment Instructions

After running the program, follow these manual post-deployment steps to complete the setup:

1. **Update GitLab CI/CD Configuration**
    - Modify your `.gitlab-ci.yml` file to include the `ARM_CLIENT_ID` printed in the console after deployment.
    - Example:
      ```sh
      variables:
        ARM_CLIENT_ID: "1761ce9f-cce0-4b77-a7f6-00f72340e063"
      ```

2. **Commit the Changes**
    - The program makes modifications to the Pulumi project. Ensure all changes are committed.

3. **Merge Changes & Trigger CI/CD Deployment**
    - Push the changes and create a merge request.
    - Merge the changes into the main branch and allow the CI/CD pipeline to handle the deployment.

Once the CI/CD pipeline successfully completes, your Pulumi stack will be fully deployed! 🚀
Future deployments will leverage federated authentication, eliminating the need for a Global Administrator.