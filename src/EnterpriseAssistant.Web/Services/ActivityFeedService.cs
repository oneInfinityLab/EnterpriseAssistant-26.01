namespace EnterpriseAssistant.Web.Services;

using EnterpriseAssistant.Core.Models.Dashboard;

/// <summary>
/// Business Logic:
/// Maintains dashboard activity events
/// generated during application execution.
///
/// Activity events are displayed within
/// the Assistant Activity dashboard panel.
/// </summary>
public sealed class ActivityFeedService
{
    private readonly IList<ActivityFeedItemDto>
        _activities =
            new List<ActivityFeedItemDto>();

    /// <summary>
    /// Business Logic:
    /// Records a new dashboard activity event.
    /// </summary>
    public void AddActivity(
        string message)
    {
        _activities.Add(
            new ActivityFeedItemDto
            {
                Timestamp = DateTime.UtcNow,
                Message = message
            });

        while (_activities.Count > 50)
        {
            _activities.RemoveAt(0);
        }
    }

    /// <summary>
    /// Business Logic:
    /// Returns the most recent dashboard
    /// activity events.
    /// </summary>
    public IReadOnlyList<ActivityFeedItemDto>
    GetActivities()
    {
        return _activities
            .Reverse()
            .Take(20)
            .ToList();
    }
}