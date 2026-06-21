namespace EnterpriseAssistant.Core.Models.Dashboard;

/// <summary>
/// Business Logic:
/// Represents operational health
/// information for a plugin.
/// </summary>
public sealed class PluginHealthDto
{
    /// <summary>
    /// Plugin display name.
    /// </summary>
    public string PluginName { get; set; } = string.Empty;

    /// <summary>
    /// Current plugin status.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}