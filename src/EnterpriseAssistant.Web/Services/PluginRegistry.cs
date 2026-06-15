namespace EnterpriseAssistant.Web.Services;

using System.Collections.Generic;
using EnterpriseAssistant.Core.Models;

/// <summary>
/// Business Logic: Registry of available plugins and their metadata.
/// Provides centralized access to plugin definitions and enablement status.
/// Acts as a read-only view of registered plugins managed by PluginRegistrationService.
/// </summary>
public sealed class PluginRegistry
{
    private readonly IList<PluginMetadata> _registeredPlugins = new List<PluginMetadata>();

    public PluginRegistry(PluginRegistrationService registrationService)
    {
        // Business Logic: Initialize registry from the registration service.
        // This ensures all registered plugins are available for querying.
        var plugins = registrationService.GetRegisteredPlugins();
        foreach (var plugin in plugins)
        {
            _registeredPlugins.Add(plugin);
        }
    }

    /// <summary>
    /// Gets the registered plugin metadata definitions.
    /// </summary>
    public IReadOnlyCollection<PluginMetadata> RegisteredPlugins => (IReadOnlyCollection<PluginMetadata>)_registeredPlugins;

    /// <summary>
    /// Gets enabled plugins only.
    /// </summary>
    public IEnumerable<PluginMetadata> EnabledPlugins => _registeredPlugins.Where(p => p.IsEnabled);
}
