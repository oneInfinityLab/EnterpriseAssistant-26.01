namespace EnterpriseAssistant.Core.Models;

using System;

/// <summary>
/// Represents the result of a weekend exclusion workflow operation.
/// </summary>
public sealed class WeekendExclusionResponse
{
    public string Id { get; init; } = string.Empty;
    public string ApplicationName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string RequestedBy { get; init; } = string.Empty;
    public DateTime RequestedDate { get; init; }
}
