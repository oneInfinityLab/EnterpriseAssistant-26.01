namespace EnterpriseAssistant.Plugins;

using EnterpriseAssistant.Core.Models;
using EnterpriseAssistant.Infrastructure.Authentication;
using EnterpriseAssistant.Infrastructure.Data;
using System.Collections.Generic;
using System.Security.Claims;

/// <summary>
/// Business Logic: Weekend exclusion workflow plugin.
/// Handles requests for weekend exclusion and review of submitted exclusions.
/// </summary>
public sealed class WeekendExclusionPlugin : IPlugin
{
    private readonly InMemoryWeekendExclusionRepository _repository;
    private readonly IUserContextService _userContextService;

    public string Name => nameof(WeekendExclusionPlugin);

    public WeekendExclusionPlugin(InMemoryWeekendExclusionRepository repository, IUserContextService userContextService)
    {
        _repository = repository;
        _userContextService = userContextService;
    }

    public WeekendExclusionResponse CreateWeekendExclusion(WeekendExclusionRequest request, ClaimsPrincipal user)
    {
        var userContext = _userContextService.GetUserContext(user);
        return _repository.CreateWeekendExclusion(request, userContext.UserName);
    }

    public IReadOnlyList<WeekendExclusionResponse> GetWeekendExclusions(ClaimsPrincipal user)
    {
        var userContext = _userContextService.GetUserContext(user);
        return _repository.GetWeekendExclusionsByRequester(userContext.UserName);
    }
}
