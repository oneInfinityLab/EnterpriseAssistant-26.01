namespace EnterpriseAssistant.Web.Controllers;

using EnterpriseAssistant.Core.Models;
using EnterpriseAssistant.Plugins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Business Logic:
/// Handles enterprise workflow operations such as
/// Issue Management, POC Requests, and Weekend Exclusions.
///
/// This controller provides a dedicated API layer
/// between the UI and workflow plugins.
/// </summary>
[ApiController]
[Route("api/workflow")]
[Authorize]
public sealed class WorkflowController : ControllerBase
{
    private readonly IssuePlugin _issuePlugin;
    private readonly PocPlugin _pocPlugin;
    private readonly WeekendExclusionPlugin _weekendPlugin;

    public WorkflowController(
        IssuePlugin issuePlugin,
        PocPlugin pocPlugin,
        WeekendExclusionPlugin weekendPlugin)
    {
        _issuePlugin = issuePlugin;
        _pocPlugin = pocPlugin;
        _weekendPlugin = weekendPlugin;
    }

    /// <summary>
    /// Business Logic:
    /// Creates a new Issue request and returns
    /// the resulting Issue details.
    /// </summary>
    [HttpPost("issue")]
    public IActionResult CreateIssue(
        [FromBody] IssueRequest request)
    {
        var response =
            _issuePlugin.CreateIssue(
                request,
                User);

        return Ok(response);
    }

    /// <summary>
    /// Business Logic:
    /// Creates a new Proof Of Concept request.
    /// </summary>
    [HttpPost("poc")]
    public IActionResult CreatePoc(
        [FromBody] PocRequest request)
    {
        var response =
            _pocPlugin.CreatePoc(
                request,
                User);

        return Ok(response);
    }

    /// <summary>
    /// Business Logic:
    /// Creates a new Weekend Exclusion request.
    /// </summary>
    [HttpPost("weekend")]
    public IActionResult CreateWeekendExclusion(
        [FromBody] WeekendExclusionRequest request)
    {
        var response =
            _weekendPlugin.CreateWeekendExclusion(
                request,
                User);

        return Ok(response);
    }
}