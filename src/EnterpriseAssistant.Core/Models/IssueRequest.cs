namespace EnterpriseAssistant.Core.Models;

/// <summary>
/// Represents a request to create an enterprise issue.
/// </summary>
public sealed class IssueRequest
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
