namespace EnterpriseAssistant.Core.Models;

/// <summary>
/// Represents the result of a plugin or knowledge provider operation.
/// </summary>
public sealed class PluginResult
{
    public bool Success { get; init; }
    public string Source { get; init; } = string.Empty;

    /// <summary>
    /// Data returned by the plugin, if any.
    /// </summary>
    public string? Data { get; init; }

    /// <summary>
    /// Error or validation message when the operation does not succeed.
    /// </summary>
    public string? Error { get; init; }
}
