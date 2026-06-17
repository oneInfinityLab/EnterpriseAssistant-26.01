namespace EnterpriseAssistant.Infrastructure.Data;

using EnterpriseAssistant.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Business Logic: In-memory storage for weekend exclusion requests.
/// Stores weekend exclusions for the current application runtime only.
/// </summary>
public sealed class InMemoryWeekendExclusionRepository
{
    private readonly List<WeekendExclusionResponse> _exclusions = new();

    public WeekendExclusionResponse CreateWeekendExclusion(WeekendExclusionRequest request, string requestedBy)
    {
        var exclusion = new WeekendExclusionResponse
        {
            Id = Guid.NewGuid().ToString(),
            ApplicationName = request.ApplicationName,
            Status = "Pending",
            RequestedBy = requestedBy,
            RequestedDate = DateTime.UtcNow
        };

        _exclusions.Add(exclusion);
        return exclusion;
    }

    public IReadOnlyList<WeekendExclusionResponse> GetWeekendExclusions()
    {
        return _exclusions.ToList();
    }

    public IReadOnlyList<WeekendExclusionResponse> GetWeekendExclusionsByRequester(string requestedBy)
    {
        return _exclusions
            .Where(exclusion => exclusion.RequestedBy.Equals(requestedBy, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
