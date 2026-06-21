namespace EnterpriseAssistant.Core.Models;

using System;

/// <summary>
/// Represents the result of an issue workflow operation.
/// </summary>
public sealed class IssueResponse
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    public string Priority { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
}
