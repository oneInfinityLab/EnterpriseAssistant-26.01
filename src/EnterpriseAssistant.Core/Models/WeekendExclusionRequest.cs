namespace EnterpriseAssistant.Core.Models;

/// <summary>
/// Represents a request for a weekend exclusion.
/// </summary>
public sealed class WeekendExclusionRequest
{
    public string ApplicationName { get; init; } = string.Empty;
}
