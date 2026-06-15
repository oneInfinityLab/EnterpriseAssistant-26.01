namespace EnterpriseAssistant.Web.Controllers;

using EnterpriseAssistant.Web.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    /// <summary>
    /// Returns service health status.
    /// </summary>
    [HttpGet]
    public IActionResult Get() => Ok(new HealthResponse { Status = "healthy" });
}
