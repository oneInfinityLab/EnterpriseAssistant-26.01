namespace EnterpriseAssistant.Core.Models.Dashboard;

/// <summary>
/// Business Logic:
/// Represents a workflow item displayed
/// in the Recent Requests dashboard.
/// </summary>
public sealed class RecentRequestDto
{
    /// <summary>
    /// Workflow type.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Workflow identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Workflow title or summary.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}