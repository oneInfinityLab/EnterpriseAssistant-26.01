namespace EnterpriseAssistant.Plugins;

using EnterpriseAssistant.Core.Interfaces;
using EnterpriseAssistant.Core.Models;
using EnterpriseAssistant.Infrastructure.Authentication;
using EnterpriseAssistant.Infrastructure.Data;
using System.Collections.Generic;
using System.Security.Claims;

/// <summary>
/// Business Logic: Issue workflow plugin.
/// Handles issue creation, status retrieval, and user issue history.
/// </summary>
public sealed class IssuePlugin : IPlugin
{
    private readonly InMemoryIssueRepository _repository;
    private readonly IUserContextService _userContextService;

    public string Name => nameof(IssuePlugin);
    /// <summary>
    /// Business Logic:
    /// Describes the primary purpose of the plugin
    /// for discovery and orchestration scenarios.
    /// </summary>
    public string Description =>
        "Creates, tracks, and manages enterprise support issues.";

    public IReadOnlyList<string> Keywords =>
    [
        "issue",
    "incident",
    "ticket",
    "support",
    "problem"
    ];

    public IssuePlugin(InMemoryIssueRepository repository, IUserContextService userContextService)
    {
        _repository = repository;
        _userContextService = userContextService;
    }

    public IssueResponse CreateIssue(IssueRequest request, ClaimsPrincipal user)
    {
        var userContext = _userContextService.GetUserContext(user);
        return _repository.CreateIssue(request, userContext.UserName);
    }

    public IssueResponse? GetIssueStatus(string id)
    {
        return _repository.GetIssueById(id);
    }

    public IReadOnlyList<IssueResponse> GetMyIssues(ClaimsPrincipal user)
    {
        var userContext = _userContextService.GetUserContext(user);
        return _repository.GetIssuesByCreator(userContext.UserName);
    }
}