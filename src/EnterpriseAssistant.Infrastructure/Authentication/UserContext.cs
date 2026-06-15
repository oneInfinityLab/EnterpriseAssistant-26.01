namespace EnterpriseAssistant.Infrastructure.Authentication;

/// <summary>
/// Represents the authenticated user context extracted from security claims.
/// </summary>
public sealed class UserContext
{
    public string UserId { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
