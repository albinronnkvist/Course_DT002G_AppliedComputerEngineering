# TIamAzureRepositoryName

## Overview

This project provides an Infrastructure as Code (IaC) template for managing identity and access management (IAM) resources in Azure. It simplifies the provisioning of resource providers, resource groups, managed identities, and role-based access control (RBAC) assignments.

### Projects

- **Pulumi:** The source of truth for IAM resources.
- **Pulumi.Test:** Unit tests for the Pulumi project.
- **Pulumi.Contract:** Contracts for the stack outputs.
- **Pulumi.Initializer:** Sets up the Pulumi project from scratch.

### Features

- **Resource Providers:** Automates the registration of Azure resource providers.
- **Resource groups:** Creates and manages resource groups, along with their associated applications and service principals.
- **Managed Identities**: Configures user-assigned managed identities.
- **RBAC:** Assigns Azure roles to identities with least-privilege access.
- **Stack References:** Exposes stack outputs for integration with other stacks (refer to the `Contract` NuGet for details).

_Application identities_, sourced from the external **Entra ID** stack, are granted the _Owner_ role within their respective _resource groups_. This allows them to create resources and assign roles within the group, ensuring flexibility while maintaining security boundaries. However, these identities cannot modify resources or permissions outside of their assigned group.

_Managed Identities_, on the other hand, are directly linked to specific resources and are assigned only the necessary roles required for their function, such as _Key Vault Reader_. Unlike _application identities_, _managed identities_ do not receive the _Owner_ role, as they are not responsible for provisioning resources but rather accessing them securely.

## Usage

### Initial Setup

For a complete setup from scratch, refer to the `Initializer` project's README.

### Modify, Verify, and Deploy

1. Modify the code to define the IAM infrastructure, then create a merge request.
2. The CI/CD pipeline will run tests and perform a Pulumi preview for validation.
3. Once verified, merge the changes, and the CI/CD pipeline will handle the deployment process automatically.

### Stack Outputs

The _resource groups_ and _managed identities_ are exposed as stack outputs, allowing seamless integration with other projects. These outputs enable corresponding projects to:

- Provision resources and assign roles within the designated _resource group_, ensuring identities operate within their defined scope. 
- Attach the _managed identity_ to specific resources, such as _Azure Container Apps_ or _Key Vaults_, granting them the necessary access permissions.

## Contribute

- **Builder pattern:** To ensure consistency and maintainability, reuse the provided builders for resource provisioning.
- **Configuration:** To manage multiple environments without drift, make environment-specific adjustments in the configuration files (`Pulumi.*.yaml`), rather than creating conditional flows in the code.
- **Stack outputs & contracts:** All stack outputs from `Stack` classes must be specified in the `Contract` project.