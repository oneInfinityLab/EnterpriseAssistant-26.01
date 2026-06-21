namespace EnterpriseAssistant.Core.Models;

/// <summary>
/// Represents a request to create an issue.
/// </summary>
public sealed class IssueRequest
{
    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Priority { get; init; } = "Medium";
}