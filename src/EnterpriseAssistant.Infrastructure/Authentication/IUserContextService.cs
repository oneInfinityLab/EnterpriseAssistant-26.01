namespace EnterpriseAssistant.Infrastructure.Authentication;

using System.Security.Claims;

public interface IUserContextService
{
    UserContext GetUserContext(ClaimsPrincipal user);
}
