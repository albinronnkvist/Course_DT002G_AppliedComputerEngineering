# TMicroserviceRepositoryName

This repository contains a containerized ASP.NET Core Web API microservice designed for secure, cloud-native deployment on Azure. The microservice is integrated with Azure Key Vault using a managed identity, enabling secure secret retrieval without embedding credentials.

## 🚀 Deployment

### 🔑 FIC Authentication with Application Client ID
This microservice is deployed via a GitLab CI/CD pipeline that uses Federated Identity Credential (FIC) authentication to securely access Azure and automate container deployment.

### 📦 Pushing to Azure Container Registry (ACR)

Once authenticated, the pipeline:

- Builds and pushes the microservice API to Azure Container Registry as a container image. 
- Tags the image using the current Git commit SHA ($CI_COMMIT_SHA).

### 🔁 Triggering Deployment via Merge Request (MR)

After publishing the image, the pipeline:

- Clones the `TContainerizationDeploymentRepositoryName` GitLab repository. 
- Updates the `Pulumi.dev.yaml` file to reference the new image tag. 
- Commits the change on a new branch.
- Creates a merge request via the GitLab CLI (glab) to deploy the new version.