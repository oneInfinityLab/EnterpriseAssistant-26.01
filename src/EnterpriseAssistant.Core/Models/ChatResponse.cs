namespace EnterpriseAssistant.Core.Models;

using System.Collections.Generic;

/// <summary>
/// Represents a response from the chat service.
/// </summary>
public sealed class ChatResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Response metadata for tracking or diagnostics.
    /// </summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }

    /// <summary>
    /// Optional details from plugin evaluations executed as part of the chat flow.
    /// </summary>
    public IEnumerable<PluginResult>? PluginActions { get; init; }
}
