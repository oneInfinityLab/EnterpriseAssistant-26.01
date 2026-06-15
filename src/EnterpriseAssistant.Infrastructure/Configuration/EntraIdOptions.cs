namespace EnterpriseAssistant.Infrastructure.Configuration;

/// <summary>
/// Configuration options for Entra ID authentication.
/// </summary>
public sealed class EntraIdOptions
{
    public string TenantId { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
}
