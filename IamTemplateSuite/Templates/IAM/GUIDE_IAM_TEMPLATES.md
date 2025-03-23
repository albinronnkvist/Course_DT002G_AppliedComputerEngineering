# 📘 Guide - IAM Templates

This section explains how to set up the two IAM templates: `IamEntraId` and `IamAzure`.

> 📝 You’ll use your previously recorded `T*` values from the [Prerequisites](../../GUIDE_PREREQUISITES.md) and [Platform Templates](../Platform/GUIDE_PLATFORM_TEMPLATES.md) guides throughout these steps.
> You'll also be asked to record further `T*` values (e.g., `TIamEntraIdRepositoryName`, `TIamAzureRepositoryName`).

---

## 📂 1. `IamEntraId` Template Setup

### 1.1. Create the GitLab Project

- Create a blank GitLab project
    - 📝 Record the **Project Name** as `TIamEntraIdRepositoryName` (e.g., `IamEntraId`)
    - 📝 Record the **Project Slug** as `TIamEntraIdRepositoryGitLabPath` (e.g., `iam-entra-id`)
- Go to **Settings → CI/CD → Job token permissions**
    - Add your GitLab group to the CI/CD job token allowlist using `TOrgGitLabPath` (e.g., `acme`)

### 1.2. Initialize the Git Repository

```bash
git clone git@gitlab.com:<TOrgGitLabPath>/<TIamEntraIdRepositoryGitLabPath>.git
cd <project>
git checkout -b main
git commit --allow-empty -m "Initialize main branch"
git push -u origin main
git checkout -b feature/setup-template
```

### 1.3. Generate the project

Generate the project using your `T*` values:

```bash
dotnet new iam-entra-id \
  --TOrgName "Acme" \
  --TOrgGitLabPath "acme" \
  --TOrgPackageSourceKey "acme.com" \
  --TOrgPackageSourceValue "https://gitlab.com/api/v4/groups/111111111/-/packages/nuget/index.json" \
  --TPlatformPulumiRepositoryName "PlatformPulumi" \
  --TPlatformPulumiAzureRepositoryName "PlatformPulumiAzure" \
  --TIamEntraIdRepositoryName "IamEntraId" \
  --TIamEntraIdRepositoryGitLabPath "iam-entra-id" \
  --TProjectPackageSourceValue "https://gitlab.com/api/v4/projects/111111114/packages/nuget/index.json" \
  --TDomainName "acme.onmicrosoft.com" \
  --TIamAzureRepositoryName "IamAzure" \
  --TIamAzureRepositoryGitLabPath "iam-azure" \
  --TContainerizationRepositoryName "Containerization" \
  --TContainerizationRepositoryGitLabPath "containerization" \
  --TContainerizationDeploymentRepositoryGitLabPath "containerization-deployment" \
  --TMicroserviceRepositoryName "Microservice" \
  --TMicroserviceRepositoryGitLabPath "microservice" \
  --TMicroserviceInfrastructureRepositoryGitLabPath "microservice-infrastructure" 
```

- `TProjectPackageSourceValue` is the **Project-level NuGet Registry URL**  
  (`https://gitlab.com/api/v4/projects/<project-id>/packages/nuget/index.json`)
- The values: `TIamAzure*`, `TContainerization*`, and `TMicroservice*`, are other projects that will be generated in subsequent steps. However, this template provisions identities for them and must therefor be specified here.

### 1.4. Finalize

- Run the generated `Pulumi.Initializer` project.
  - Refer to the `Pulumi.Initializer` project's `README.md` for prerequisites and setup instructions.
  - Confirm the Initializer completes successfully and follow any post-deployment steps printed in the output.
- Commit all generated and modified files.
- Push your feature branch to the remote repository.
- Create a Merge Request and wait for the CI pipeline to complete to validate the changes.
- Once merged, the CI/CD pipeline will automatically: build and test the project, deploy resources, and publish a NuGet package.

---

## 📂 2. `IamAzure` Template Setup

### 2.1. Create the GitLab Project

- Create a blank GitLab project
    - 📝 Record the **Project Name** as `TIamAzureRepositoryName` (e.g., `IamAzure`)
    - 📝 Record the **Project Slug** as `TIamAzureRepositoryGitLabPath` (e.g., `iam-azure`)
- Go to **Settings → CI/CD → Job token permissions**
    - Add your GitLab group to the CI/CD job token allowlist using `TOrgGitLabPath` (e.g., `acme`)

### 2.2. Initialize the Git Repository

```bash
git clone git@gitlab.com:<TOrgGitLabPath>/<TIamAzureRepositoryGitLabPath>.git
cd <project>
git checkout -b main
git commit --allow-empty -m "Initialize main branch"
git push -u origin main
git checkout -b feature/setup-template
```

### 2.3. Generate the project

Generate the project using your `T*` values:

```bash
dotnet new iam-azure \
  --TOrgName "Acme" \
  --TOrgGitLabPath "acme" \
  --TOrgPackageSourceKey "acme.com" \
  --TOrgPackageSourceValue "https://gitlab.com/api/v4/groups/111111111/-/packages/nuget/index.json" \
  --TPlatformPulumiRepositoryName "PlatformPulumi" \
  --TPlatformPulumiAzureRepositoryName "PlatformPulumiAzure" \
  --TIamEntraIdRepositoryName "IamEntraId" \
  --TIamAzureRepositoryName "IamAzure" \
  --TIamAzureRepositoryGitLabPath "iam-azure" \
  --TProjectPackageSourceValue "https://gitlab.com/api/v4/projects/111111114/packages/nuget/index.json" \
  --TContainerizationRepositoryName "Containerization" \
  --TContainerizationRepositoryGitLabPath "containerization" \
  --TContainerizationDeploymentRepositoryGitLabPath "containerization-deployment" \
  --TMicroserviceRepositoryName "Microservice" \
  --TMicroserviceRepositoryGitLabPath "microservice" \
  --TMicroserviceInfrastructureRepositoryGitLabPath "microservice-infrastructure" 
```

- `TProjectPackageSourceValue` is the **Project-level NuGet Registry URL** for this specific project (`iam-azure`), and will be different from `iam-entra-id` above.

### 2.4. Finalize

- Run the generated `Pulumi.Initializer` project.
  - Refer to the `Pulumi.Initializer` project's `README.md` for prerequisites and setup instructions.
  - Confirm the Initializer completes successfully and follow any post-deployment steps printed in the output.
- Commit all generated and modified files.
- Push your feature branch to the remote repository.
- Create a Merge Request and wait for the CI pipeline to complete to validate the changes.
- Once merged, the CI/CD pipeline will automatically: build and test the project, deploy resources, and publish NuGet packages.

---

## 📝 Template Values for Subsequent Steps

Ensure the following values are recorded, in addition to the values from the [Prerequisites](../../GUIDE_PREREQUISITES.md) and [Platform Templates](../Platform/GUIDE_PLATFORM_TEMPLATES.md) guides.

| Key                                               | Description                                                                                    |
|---------------------------------------------------|------------------------------------------------------------------------------------------------|
| `TIamEntraIdRepositoryName`                       | Project name for the IamEntraId repository (e.g., `IamEntraId`)                                |
| `TIamAzureRepositoryName`                         | Project name for the IamAzure repository (e.g., `IamAzure`)                                    |
| `TContainerizationRepositoryName`                 | Project name for the Containerization repository (e.g., `Containerization`)                    |
| `TContainerizationRepositoryGitLabPath`           | GitLab path/slug for the Containerization repository (e.g., `containerization`)                |
| `TContainerizationDeploymentRepositoryGitLabPath` | GitLab path/slug for the ContainerizationDeployment repo (e.g., `containerization-deployment`) |
| `TMicroserviceRepositoryName`                     | Project name for the Microservice repository (e.g., `Microservice`)                            |
| `TMicroserviceRepositoryGitLabPath`               | GitLab path/slug for the Microservice repository (e.g., `microservice`)                        |
| `TMicroserviceInfrastructureRepositoryGitLabPath` | GitLab path/slug for the MicroserviceInfrastructure repo (e.g., `microservice-infrastructure`) |