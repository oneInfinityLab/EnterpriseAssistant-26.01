namespace EnterpriseAssistant.Web.Controllers;

using EnterpriseAssistant.Core.Models;
using EnterpriseAssistant.Core.Models.Dashboard;
using EnterpriseAssistant.Web.Services;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Business Logic:
/// Provides dashboard related APIs for the
/// Enterprise Assistant user interface.
///
/// Dashboard endpoints expose workflow metrics
/// and operational information required by
/// dashboard widgets and monitoring panels.
/// </summary>
[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    /// <summary>
    /// Business Logic:
    /// Creates a dashboard controller capable
    /// of serving workflow metrics and dashboard
    /// information to the web application.
    /// </summary>
    public DashboardController(
        DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Business Logic:
    /// Retrieves workflow metrics used by the
    /// dashboard metrics panel.
    ///
    /// Returns aggregate counts for:
    /// - Issues
    /// - POCs
    /// - Weekend Exclusions
    /// </summary>
    [HttpGet("metrics")]
    [ProducesResponseType(typeof(DashboardMetricsResponse), StatusCodes.Status200OK)]
    public ActionResult<DashboardMetricsResponse> GetMetrics()
    {
        var metrics =
            _dashboardService.GetMetrics();

        return Ok(metrics);
    }

    /// <summary>
    /// Business Logic:
    /// Retrieves recently executed workflow
    /// requests for dashboard display.
    /// </summary>
    [HttpGet("recent-requests")]
    [ProducesResponseType(
        typeof(IReadOnlyList<RecentRequestDto>),
        StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<RecentRequestDto>>
    GetRecentRequests()
    {
        return Ok(
            _dashboardService.GetRecentRequests());
    }
    
    /// <summary>
    /// Business Logic:
    /// Returns recent Enterprise Assistant
    /// activity events for dashboard display.
    /// </summary>
    [HttpGet("activity-feed")]
    public IActionResult GetActivityFeed()
    {
        return Ok(
            _dashboardService
                .GetActivityFeed());
    }
}