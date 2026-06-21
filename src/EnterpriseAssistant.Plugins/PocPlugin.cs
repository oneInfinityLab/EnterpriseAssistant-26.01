namespace EnterpriseAssistant.Plugins;

using EnterpriseAssistant.Core.Interfaces;
using EnterpriseAssistant.Core.Models;
using EnterpriseAssistant.Infrastructure.Authentication;
using EnterpriseAssistant.Infrastructure.Data;
using System.Collections.Generic;
using System.Security.Claims;

/// <summary>
/// Business Logic: Proof-of-concept workflow plugin.
/// Handles POC request submission and requester history.
/// </summary>
public sealed class PocPlugin : IPlugin
{
    private readonly InMemoryPocRepository _repository;
    private readonly IUserContextService _userContextService;

    public string Name => nameof(PocPlugin);

    public string Description =>
    "Handles proof of concept request workflows.";

    public IReadOnlyList<string> Keywords =>
    [
        "poc",
    "demo",
    "prototype",
    "customer"
    ];
    public PocPlugin(InMemoryPocRepository repository, IUserContextService userContextService)
    {
        _repository = repository;
        _userContextService = userContextService;
    }

    public PocResponse CreatePoc(PocRequest request, ClaimsPrincipal user)
    {
        var userContext = _userContextService.GetUserContext(user);
        return _repository.CreatePoc(request, userContext.UserName);
    }

    public PocResponse? GetPocById(string id)
    {
        return _repository.GetPocById(id);
    }

    public IReadOnlyList<PocResponse> GetMyPocs(ClaimsPrincipal user)
    {
        var userContext = _userContextService.GetUserContext(user);
        return _repository.GetPocsByRequester(userContext.UserName);
    }
}
