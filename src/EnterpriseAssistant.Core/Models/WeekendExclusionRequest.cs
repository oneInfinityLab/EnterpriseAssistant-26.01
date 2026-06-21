namespace EnterpriseAssistant.Core.Models;

/// <summary>
/// Represents a request for a weekend exclusion.
/// </summary>
public sealed class WeekendExclusionRequest
{
    public string ChangeRequest { get; init; } = string.Empty;

    public string WeekendDate { get; init; } = string.Empty;

    public string Justification { get; init; } = string.Empty;
    public string ApplicationName { get; set; }
}