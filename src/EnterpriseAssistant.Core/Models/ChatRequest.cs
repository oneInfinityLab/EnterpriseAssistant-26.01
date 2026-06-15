namespace EnterpriseAssistant.Core.Models;

using System.Collections.Generic;

/// <summary>
/// Represents a request to the chat service.
/// </summary>
public sealed class ChatRequest
{
    public string UserId { get; init; } = string.Empty;
    public string Prompt { get; init; } = string.Empty;

    /// <summary>
    /// Optional metadata supplied with the request.
    /// </summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }

    /// <summary>
    /// External session identifier for correlation.
    /// </summary>
    public string? SessionId { get; init; }
}
