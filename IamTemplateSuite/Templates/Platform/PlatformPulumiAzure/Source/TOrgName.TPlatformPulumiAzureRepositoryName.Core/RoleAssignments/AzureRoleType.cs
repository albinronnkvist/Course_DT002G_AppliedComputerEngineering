using Ardalis.SmartEnum;

namespace TOrgName.TPlatformPulumiAzureRepositoryName.Core.RoleAssignments;

public sealed class AzureRoleType : SmartEnum<AzureRoleType>
{
    public static readonly AzureRoleType Owner = new("owner", 1, "/providers/Microsoft.Authorization/roleDefinitions/8e3af657-a8ff-443c-a75c-2fe8c4bcb635");
    public static readonly AzureRoleType Contributor = new("contributor", 2, "/providers/Microsoft.Authorization/roleDefinitions/b24988ac-6180-42a0-ab88-20f7382dd24c");
    public static readonly AzureRoleType KeyVaultAdministrator = new("key-vault-administrator", 3, "/providers/Microsoft.Authorization/roleDefinitions/00482a5a-887f-4fb3-b363-3b7fe8e74483");
    public static readonly AzureRoleType KeyVaultSecretsUser = new("key-vault-secrets-user", 4, "/providers/Microsoft.Authorization/roleDefinitions/4633458b-17de-408a-b874-0445c86b69e6");
    public static readonly AzureRoleType AcrPush = new("acr-push", 5, "/providers/Microsoft.Authorization/roleDefinitions/8311e382-0749-4cb8-b61a-304f252e45ec");
    public static readonly AzureRoleType AcrPull = new("acr-pull", 6, "/providers/Microsoft.Authorization/roleDefinitions/7f951dda-4ed3-4680-a7ca-43fe172d538d");
    public static readonly AzureRoleType ContainerAppsContributor = new("container-apps-contributor", 7, "/providers/Microsoft.Authorization/roleDefinitions/358470bc-b998-42bd-ab17-a7e34c199c0f");
    
    public string RoleDefinitionId { get; }

    private AzureRoleType(string name, int value, string roleDefinitionId) : base(name, value) => 
        RoleDefinitionId = roleDefinitionId;
}
