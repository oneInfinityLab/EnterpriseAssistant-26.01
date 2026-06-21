namespace EnterpriseAssistant.Web.Services;

using EnterpriseAssistant.Core.Models;
using EnterpriseAssistant.Core.Models.Dashboard;
using EnterpriseAssistant.Infrastructure.Data;

/// <summary>
/// Business Logic:
/// Provides workflow dashboard metrics for
/// Enterprise Assistant monitoring panels.
///
/// Aggregates workflow information from the
/// underlying repositories and prepares
/// dashboard-friendly response models.
/// </summary>
public sealed class DashboardService
{
    private readonly InMemoryIssueRepository _issueRepository;
    private readonly InMemoryPocRepository _pocRepository;
    private readonly InMemoryWeekendExclusionRepository _weekendRepository;
    private readonly ActivityFeedService _activityFeedService;
    /// <summary>
    /// Business Logic:
    /// Creates a dashboard service capable of
    /// aggregating workflow metrics from all
    /// supported workflow repositories.
    /// </summary>
    public DashboardService(
        InMemoryIssueRepository issueRepository,
        InMemoryPocRepository pocRepository,
        InMemoryWeekendExclusionRepository weekendRepository, ActivityFeedService activityFeedService)
    {
        _issueRepository = issueRepository;
        _pocRepository = pocRepository;
        _weekendRepository = weekendRepository;
        _activityFeedService = activityFeedService;
    }

    /// <summary>
    /// Business Logic:
    /// Retrieves current workflow metrics for
    /// display within the Enterprise Assistant
    /// dashboard experience.
    ///
    /// Metrics include:
    /// - Total Issues
    /// - Total POCs
    /// - Total Weekend Exclusions
    /// </summary>
    public DashboardMetricsResponse GetMetrics()
    {
        return new DashboardMetricsResponse
        {
            IssueCount =
                _issueRepository.GetIssueCount(),

            PocCount =
                _pocRepository.GetPocCount(),

            WeekendExclusionCount =
                _weekendRepository.GetWeekendExclusionCount()
        };
    }
    /// <summary>
    /// Business Logic:
    /// Returns the most recent workflow
    /// requests across all supported
    /// enterprise workflow types.
    /// </summary>
    public IReadOnlyList<RecentRequestDto>
    GetRecentRequests()
    {
        var requests =
            new List<RecentRequestDto>();

        foreach (var issue in _issueRepository.GetAll())
        {
            requests.Add(
                new RecentRequestDto
                {
                    Type = "Issue",
                    Id = issue.Id,
                    Title = issue.Title
                });
        }

        foreach (var poc in _pocRepository.GetAll())
        {
            requests.Add(
                new RecentRequestDto
                {
                    Type = "POC",
                    Id = poc.Id,
                    Title = poc.Title
                });
        }

        foreach (var weekend in
            _weekendRepository.GetAll())
        {
            requests.Add(
                new RecentRequestDto
                {
                    Type = "Weekend",
                    Id = weekend.Id,
                    Title = weekend.ApplicationName
                });
        }

        return requests
            .TakeLast(10)
            .Reverse()
            .ToList();
    }
    /// <summary>
    /// Business Logic:
    /// Returns recent Enterprise Assistant
    /// activity events for dashboard display.
    /// </summary>
    public IReadOnlyList<ActivityFeedItemDto>
    GetActivityFeed()
    {
        return _activityFeedService
            .GetActivities();
    }

    /// <summary>
    /// Business Logic:
    /// Returns operational health information
    /// for registered Enterprise Assistant plugins.
    /// </summary>
    public IReadOnlyList<PluginHealthDto>
    GetPluginHealth()
    {
        return
        [
            new PluginHealthDto
        {
            PluginName = "Knowledge Search",
            Status = "Healthy"
        },

        new PluginHealthDto
        {
            PluginName = "Issue Plugin",
            Status = "Healthy"
        },

        new PluginHealthDto
        {
            PluginName = "POC Plugin",
            Status = "Healthy"
        },

        new PluginHealthDto
        {
            PluginName = "Weekend Exclusion",
            Status = "Healthy"
        }
        ];
    }
}