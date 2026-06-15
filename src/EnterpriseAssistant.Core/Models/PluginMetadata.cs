namespace EnterpriseAssistant.Core.Models;

/// <summary>
/// Represents metadata describing a registered plugin.
/// Provides information about plugin capabilities, versioning, and enablement status.
/// </summary>
public sealed class PluginMetadata
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public bool IsEnabled { get; init; }
}
