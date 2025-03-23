# 📘 Guide - Prerequisites

This guide walks you through setting up the required tools and accounts to use the template suite effectively.

> ⚠️ Throughout this guide, you'll be asked to **note down key configuration values** prefixed with `T*` (e.g., `TOrgName`, `TDomainName`).  
> These values are **used consistently across templates**, so be sure to record and use them consistently.

---

## ☁️ Entra ID & Azure Setup

To provision cloud resources, first set up your Azure environment:

### 1. Create Required Accounts

- [Create a Microsoft account](https://signup.live.com/) (if you don’t have one already)
- [Create an Azure account](https://azure.microsoft.com/en-us/pricing/purchase-options/azure-account?icid=azurefreeaccount&WT.mc_id=A261C142F)

Upon Azure signup:
- A **Tenant** will be automatically created under *Microsoft Entra ID* (called "Default Directory")
  - 📝 Record the **Tenant ID** as `TTenantId` (e.g., `0dab5c50-01cc-4c65-834c-52d042b8ac9e`)
  - 📝 Record the **Tenant Domain Name** as `TDomainName` (e.g., `acme.onmicrosoft.com`)
- A **Subscription** will be created under *Subscriptions* (named "Azure subscription 1")
  - 📝 Record the **Subscription ID** as `TSubscriptionId` (e.g., `39b3120a-dbfd-4aa8-ab24-88b8fe0a30bb`)

### 2. Install & Login to Azure CLI

- [Install Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- Log in with your Microsoft account (you will be a Global Administrator by default):

```bash
az login
```

---


## ⚙️ Pulumi Setup

Pulumi is the IaC tool used to provision cloud infrastructure.

### 1. Create an Account & Personal Access Token (PAT)

- [Sign up for Pulumi](https://app.pulumi.com/signup)
- Verify your email
- Go to **Account → Personal Access Tokens**, create a new token
    - Remember your `PULUMI_ACCESS_TOKEN` (e.g., `pul-abc123...`) — it won't be shown again!

### 2. Install & Login to Pulumi CLI

- [Install the Pulumi CLI](https://www.pulumi.com/docs/iac/download-install/)
- Log in:

```bash
pulumi login
```

---

## 🦊 GitLab Setup

GitLab is used for version control, CI/CD, and hosting NuGet packages.

### 1. Create & Configure GitLab Account

- [Create a GitLab account](https://gitlab.com/users/sign_up)
- Verify your email and phone number (required for CI/CD)

### 2. Create a GitLab Group

- Go to **Groups → New Group**
    - 📝 Record the **Group Name** as `TOrgName` (e.g., `Acme`)
    - 📝 Record the **Group URL Path** as `TOrgGitLabPath` (e.g., `acme`)

### 3. Configure CI/CD Variables

- Navigate to **Group → Settings → CI/CD → Variables**
- Set a default minimum role (e.g., Developer)
- Add a new variable:
    - **Key**: `PULUMI_ACCESS_TOKEN`
    - **Value**: your Pulumi PAT (e.g., `pul-abc123...`)
    - Enable **Masked** and **Protected**

### 4. Set Up GitLab NuGet Authentication

- [Add an SSH key](https://docs.gitlab.com/ee/user/ssh.html) to your GitLab account
- Create a GitLab **Personal Access Token (PAT)** with the `read_api` scope
    - Remember your GitLab **username** (e.g., `acme-user`)
    - Remember your GitLab **PAT** (e.g., `glpat-xyz...`)

Then authenticate your machine to the GitLab NuGet registry:

```bash
dotnet nuget add source "TOrgPackageSourceValue" \
  --name TOrgPackageSourceKey \
  --username your_gitlab_username \
  --password your_gitlab_pat \
  --store-password-in-clear-text \
  --configfile ~/.nuget/NuGet/NuGet.Config
```

- 📝 Use and record the **GitLab private group-level NuGet Source URL** as `TOrgPackageSourceValue`  
  (e.g., `https://gitlab.com/api/v4/groups/<group-id>/-/packages/nuget/index.json`)
- 📝 Use and record the **GitLab private group-level NuGet Source Key** as `TOrgPackageSourceKey`  
  (e.g., `acme.com`)
- Use your GitLab **username** (e.g., `acme-user`) and **PAT** (e.g., `glpat-xyz...`) for authentication 

---

## 📝 Template Values for Subsequent Steps

The following values must be recorded and used consistently across configuration files, templates, and CI/CD pipelines:

| Key                      | Description                                                                                                                    |
|--------------------------|--------------------------------------------------------------------------------------------------------------------------------|
| `TTenantId`              | Entra ID Tenant ID (e.g., `0dab5c50-01cc-4c65-834c-52d042b8ac9e`)                                                              |
| `TDomainName`            | Entra ID domain (e.g., `acme.onmicrosoft.com`)                                                                                 |
| `TSubscriptionId`        | Azure Subscription ID (e.g., `39b3120a-dbfd-4aa8-ab24-88b8fe0a30bb`)                                                           |
| `TOrgName`               | GitLab group display name (e.g., `Acme`)                                                                                       |
| `TOrgGitLabPath`         | GitLab group URL path (e.g., `acme`)                                                                                           |
| `TOrgPackageSourceKey`   | GitLab private group-level NuGet registry source name (e.g., `acme.com`)                                                       |
| `TOrgPackageSourceValue` | GitLab private group-level NuGet registry URL (e.g., `https://gitlab.com/api/v4/groups/111111111/-/packages/nuget/index.json`) |
