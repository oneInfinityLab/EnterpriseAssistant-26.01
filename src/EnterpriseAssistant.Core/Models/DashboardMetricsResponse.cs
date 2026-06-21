namespace EnterpriseAssistant.Core.Models;

/// <summary>
/// Business Logic:
/// Represents workflow metrics displayed
/// on the Enterprise Assistant dashboard.
///
///— Provides aggregate counts for all
/// supported workflow types.
///— Used by dashboard APIs and UI widgets.
/// </summary>
public sealed class DashboardMetricsResponse
{
    /// <summary>
    /// Total issue requests currently stored.
    /// </summary>
    public int IssueCount { get; set; }

    /// <summary>
    /// Total proof of concept requests currently stored.
    /// </summary>
    public int PocCount { get; set; }

    /// <summary>
    /// Total weekend exclusion requests currently stored.
    /// </summary>
    public int WeekendExclusionCount { get; set; }
}