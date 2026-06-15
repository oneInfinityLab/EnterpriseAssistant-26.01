namespace EnterpriseAssistant.Web.Controllers;

using EnterpriseAssistant.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class UserController : ControllerBase
{
    private readonly IUserContextService _userContextService;

    public UserController(IUserContextService userContextService)
    {
        _userContextService = userContextService;
    }

    /// <summary>
    /// Business Logic: Get the current authenticated user's context.
    /// Returns user identification, name, and email extracted from Entra ID claims.
    /// 
    /// Requires authentication - returns 401 if user is not authenticated.
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        // Business Logic: Extract user context from the authenticated principal.
        // The [Authorize] attribute ensures only authenticated requests reach this endpoint.
        var userContext = _userContextService.GetUserContext(User);

        return Ok(userContext);
    }
}
