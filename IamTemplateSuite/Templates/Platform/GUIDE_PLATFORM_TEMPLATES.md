# 📘 Guide - Platform Templates

This section explains how to set up the two platform templates: `PlatformPulumi` and `PlatformPulumiAzure`.

> 📝 You’ll use your previously recorded `T*` values from the [Prerequisites](../../GUIDE_PREREQUISITES.md) guide throughout these steps.
> You'll also be asked to record further `T*` values (e.g., `TPlatformPulumiRepositoryName`, `TPlatformPulumiAzureRepositoryName`).

---

## 📂 1. `PlatformPulumi` Template Setup

### 1.1. Create the GitLab Project

- Create a blank GitLab project
  - 📝 Record the **Project Name** as `TPlatformPulumiRepositoryName` (e.g., `PlatformPulumi`)
  - 📝 Record the **Project Slug** as `TPlatformPulumiRepositoryGitLabPath` (e.g., `platform-pulumi`)
- Go to **Settings → CI/CD → Job token permissions**
  - Add your GitLab group to the CI/CD job token allowlist using `TOrgGitLabPath` (e.g., `acme`)

### 1.2. Initialize the Git Repository

```bash
git clone git@gitlab.com:<TOrgGitLabPath>/<TPlatformPulumiRepositoryGitLabPath>.git
cd <project>
git checkout -b main
git commit --allow-empty -m "Initialize main branch"
git push -u origin main
git checkout -b feature/setup-template
```

### 1.3. Generate the project

Generate the project using your `T*` values:

```bash
dotnet new platform-pulumi \
  --TOrgName "Acme" \
  --TOrgGitLabPath "acme" \
  --TOrgPackageSourceKey "acme.com" \
  --TOrgPackageSourceValue "https://gitlab.com/api/v4/groups/111111111/-/packages/nuget/index.json" \
  --TPlatformPulumiRepositoryName "PlatformPulumi" \
  --TPlatformPulumiRepositoryGitLabPath "platform-pulumi" \
  --TProjectPackageSourceValue "https://gitlab.com/api/v4/projects/111111112/packages/nuget/index.json"
```

- `TProjectPackageSourceValue` is the **Project-level NuGet Registry URL**  
  (e.g., `https://gitlab.com/api/v4/projects/<project-id>/packages/nuget/index.json`)

This scaffolds the `PlatformPulumi` project with your custom configuration.

### 1.4. Finalize

- Commit your changes and push the branch
- Create a merge request and wait for the CI pipeline to complete
- Merge to main:
  - The CI/CD pipeline will build, test, and publish the NuGet package to the private GitLab registry
  - Versioning is automatic unless overridden with Git tags

---

## 📂 2. `PlatformPulumiAzure` Template Setup

### 2.1. Create the GitLab Project

- Create a blank GitLab project
  - 📝 Record the **Project Name** as `TPlatformPulumiAzureRepositoryName` (e.g., `PlatformPulumiAzure`)
  - 📝 Record the **Project Slug** as `TPlatformPulumiAzureRepositoryGitLabPath` (e.g., `platform-pulumi-azure`)
- Go to **Settings → CI/CD → Job token permissions**
  - Add your GitLab group to the CI/CD job token allowlist using `TOrgGitLabPath` (e.g., `acme`)

### 2.2. Initialize the Git Repository

```bash
git clone git@gitlab.com:<TOrgGitLabPath>/<TPlatformPulumiAzureRepositoryGitLabPath>.git
cd <project>
git checkout -b main
git commit --allow-empty -m "Initialize main branch"
git push -u origin main
git checkout -b feature/setup-template
```

### 2.3. Generate the project

Generate the project using your `T*` values:

```bash
dotnet new platform-pulumi-azure \
  --TOrgName "Acme" \
  --TOrgGitLabPath "acme" \
  --TOrgPackageSourceKey "acme.com" \
  --TOrgPackageSourceValue "https://gitlab.com/api/v4/groups/111111111/-/packages/nuget/index.json" \
  --TPlatformPulumiRepositoryName "PlatformPulumi" \
  --TPlatformPulumiAzureRepositoryName "PlatformPulumiAzure" \
  --TPlatformPulumiAzureRepositoryGitLabPath "platform-pulumi-azure" \
  --TProjectPackageSourceValue "https://gitlab.com/api/v4/projects/111111113/packages/nuget/index.json"
```

### 2.4. Finalize

- Commit your changes and push the branch
- Create a merge request and wait for the CI pipeline to complete
- Merge to main:
  - The CI/CD pipeline will build, test, and publish the NuGet package to the private GitLab registry
  - Versioning is automatic unless overridden with Git tags

---

## 📝 Template Values for Subsequent Steps

Ensure the following values are recorded, in addition to the values from the [Prerequisites](../../GUIDE_PREREQUISITES.md) guide. 

| Key                                      | Description                                                                       |
|------------------------------------------|-----------------------------------------------------------------------------------|
| `TPlatformPulumiRepositoryName`          | Project name for the PlatformPulumi repository (e.g., `PlatformPulumi`)           |
| `TPlatformPulumiAzureRepositoryName`     | Project name for the PlatformPulumiAzure repository (e.g., `PlatformPulumiAzure`) |