namespace EnterpriseAssistant.Core.Models.Dashboard;

/// <summary>
/// Business Logic:
/// Represents aggregated workflow analytics
/// displayed within the operational dashboard.
/// </summary>
public sealed class WorkflowAnalyticsDto
{
    /// <summary>
    /// Total workflow requests.
    /// </summary>
    public int TotalRequests { get; set; }

    /// <summary>
    /// Most frequently used workflow.
    /// </summary>
    public string MostUsedWorkflow { get; set; } = string.Empty;

    /// <summary>
    /// Percentage of Issue requests.
    /// </summary>
    public double IssuePercentage { get; set; }

    /// <summary>
    /// Percentage of POC requests.
    /// </summary>
    public double PocPercentage { get; set; }

    /// <summary>
    /// Percentage of Weekend Exclusion requests.
    /// </summary>
    public double WeekendPercentage { get; set; }
}