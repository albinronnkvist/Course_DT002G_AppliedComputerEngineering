# TContainerizationRepositoryName

This repository defines the core Azure containerization infrastructure for TOrgName using Pulumi IaC.

This project provisions:

- **Azure Container Registry (ACR)**: A central registry where container images are securely published and managed.
- **Azure Container Apps Environment (CAE)**: An environment for running containerized applications.

These resources are pre-configured with role assignments and identity integrations, enabling collaboration between microservice pipelines and deployment tooling. 
This project does not create identities but relies on pre-provisioned managed identities and roles from upstream IAM projects.