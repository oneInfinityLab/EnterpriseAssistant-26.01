namespace EnterpriseAssistant.Core.Models;

/// <summary>
/// Represents a request to submit a proof of concept.
/// </summary>
public sealed class PocRequest
{
    public string Title { get; init; } = string.Empty;
    public string BusinessJustification { get; init; } = string.Empty;
}
