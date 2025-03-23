# TIamEntraIdRepositoryName

## Overview

This project serves as an Infrastructure as Code (IaC) template for managing identity and access management (IAM) resources in Entra ID. It streamlines the provisioning and administration of users, applications, and service principals while automating directory role assignments and permission configurations.

### Projects

- **Pulumi:** The source of truth for IAM resources.
- **Pulumi.Test:** Unit tests for the Pulumi project.
- **Pulumi.Contract:** Contracts for the stack outputs.
- **Pulumi.Initializer:** Sets up the Pulumi project from scratch.

### Features

- **User & Group Management:** Automates the creation and management of Entra ID users and groups.
- **Application & Service Principal Provisioning:** Creates and configures applications and their associated service principals.
    - **Federated Identity Credentials (FICs):** Manages FICs for external workload authentication.
- **Directory Roles & API Permissions:** Assigns directory roles and configures API permissions with automatic admin consent.
- **Stack References:** Exposes stack outputs for integration with other stacks (refer to the `Contract` NuGet for details).

Each workload identity is defined as an Entra ID _Application_ with an associated _Service Principal_ and _Federated Identity Credentials_ (you can choose between a flexible or standard FIC). 
The required _Directory Roles_ and _API Permissions_ are assigned to these identities with least-privilege access in mind. 
Additionally, _Users_ are defined and added to _Groups_ as owners or members, with the necessary _Directory Roles_.

## Usage

### Initial Setup

For a complete setup from scratch, refer to the `Initializer` project's README.

### Modify, Verify, and Deploy

1. Modify the code to define the IAM infrastructure, then create a merge request.
2. The CI/CD pipeline will run tests and perform a Pulumi preview for validation.
3. Once verified, merge the changes, and the CI/CD pipeline will handle the deployment process automatically.

### Stack Outputs

The _Application Identities_ are provided as stack outputs, facilitating seamless integration with other projects. 
These outputs enable related projects to authenticate to Azure using the _Application ClientId_ and the underlying _Service Principal_ and _FIC_.
However, Azure role provisioning is outside the scope of this project.

## Contribute

- **Builder pattern:** To ensure consistency and maintainability, reuse the provided builders for resource provisioning. 
- **Configuration:** To manage multiple environments without drift, make environment-specific adjustments in the configuration files (`Pulumi.*.yaml`), rather than creating conditional flows in the code.
- **Stack outputs & contracts:** All stack outputs from `Stack` classes must be specified in the `Contract` project.

## Other

### Issues & Workarounds

#### 1. Directory Roles (⚠️ Action required)

Pulumi encounters issues when provisioning directory roles because unassigned roles are not exposed via the Microsoft Graph API.
Azure does not consider a directory role "activated" until it has been assigned to at least one user or service principal. Since Pulumi relies on the API to retrieve role details, any unassigned role remains invisible, preventing it from being managed programmatically ([read more](https://github.com/hashicorp/terraform-provider-azuread/issues/1526)).

**Workaround:** Manually assign the directory roles (see `DirectoryRoleType`) to any user or service principal in the Azure portal, before provisioning it with Pulumi. Once assigned, Azure marks the role as activated, making it detectable via the API and manageable by Pulumi.

#### 2. Flexible Federated Identity Credentials

Currently, Flexible FICs are not supported in Pulumi (it’s a preview feature).

**Workaround:** The project leverages Pulumi's `Command` type along with Azure CLI’s `az rest` method to make direct REST API requests. 
This enables users to interact with _Flexible FICs_ programmatically until official support is available. The commands are executed using the `/bin/bash` interpreter.
