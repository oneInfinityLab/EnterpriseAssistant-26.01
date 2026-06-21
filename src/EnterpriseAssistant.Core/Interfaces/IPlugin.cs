namespace EnterpriseAssistant.Core.Interfaces;

/// <summary>
/// Business Logic:
/// Defines a discoverable Enterprise Assistant plugin.
///
/// Plugins expose metadata that can be used by the
/// Assistant Orchestrator and Plugin Registry to
/// dynamically identify available capabilities.
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// Unique plugin name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Human readable plugin description.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Keywords associated with plugin capabilities.
    /// Used by orchestration and discovery components.
    /// </summary>
    IReadOnlyList<string> Keywords { get; }
}