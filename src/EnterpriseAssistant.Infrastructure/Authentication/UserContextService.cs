namespace EnterpriseAssistant.Infrastructure.Authentication;

using System.Security.Claims;

/// <summary>
/// Business Logic: Service responsible for extracting user context from authenticated user claims.
/// Provides a unified interface to access user identity information in the application.
/// 
/// Claims are populated by Microsoft Entra ID after successful authentication.
/// User identification is based on the subject claim (sub) which is immutable across token refreshes.
/// </summary>
public sealed class UserContextService : IUserContextService
{
    /// <summary>
    /// Business Logic: Extract user context from security claims principal.
    /// Reads user identification, name, and email claims from the authenticated user.
    /// Claims come from the JWT token issued by Microsoft Entra ID.
    /// </summary>
    public UserContext GetUserContext(ClaimsPrincipal user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        // Business Logic: Extract user claims from the principal.
        // These claims are populated by Microsoft Entra ID during authentication.
        // - "sub" (subject): Unique immutable identifier for the user
        // - "name": User's display name from directory
        // - "email": Primary email address from directory
        var userIdClaim = user.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")
            ?? user.FindFirst(ClaimTypes.NameIdentifier)
            ?? user.FindFirst("sub");

        var nameClaim = user.FindFirst(ClaimTypes.Name)
            ?? user.FindFirst("name");

        var emailClaim = user.FindFirst(ClaimTypes.Email)
            ?? user.FindFirst("email");

        // Business Logic: Create UserContext with extracted claims.
        // Provide empty strings as defaults if claims are not available.
        return new UserContext
        {
            UserId = userIdClaim?.Value ?? string.Empty,
            UserName = nameClaim?.Value ?? string.Empty,
            Email = emailClaim?.Value ?? string.Empty
        };
    }
}
