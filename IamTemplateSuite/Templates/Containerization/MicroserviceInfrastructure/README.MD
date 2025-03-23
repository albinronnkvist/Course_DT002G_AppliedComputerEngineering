# TMicroserviceInfrastructureRepositoryName

This project defines the infrastructure required for the TMicroserviceRepositoryName project to securely access secrets stored in an Azure Key Vault.

Using Federated Identity Credential (FIC) authentication and pre-assigned roles, it provisions:

- An Azure Key Vault
  - 🔐 A secret stored securely inside the vault

The goal is to allow TMicroserviceRepositoryName to retrieve secrets from Key Vault without storing any credentials, using a managed identity instead.