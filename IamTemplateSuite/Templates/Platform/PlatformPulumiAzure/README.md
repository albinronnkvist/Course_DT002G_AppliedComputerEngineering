# PlatformPulumiAzure

This package extends the _PlatformPulumi_ package with Azure-specific conventions and components. 

## Versioning

The package follows semantic versioning with [GitVersion](https://gitversion.net/):

- Major and minor versions are manually controlled using [Git tags](https://git-scm.com/docs/git-tag). 
- Patch versions are automatically incremented with each build.

## CI/CD Pipeline

- Pre-release NuGet packages are generated upon manual triggers in each merge requests. 
- Release NuGet packages are automatically published when merging to main.