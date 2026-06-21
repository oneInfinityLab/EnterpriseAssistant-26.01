namespace EnterpriseAssistant.Web.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using EnterpriseAssistant.Core.Interfaces;

/// <summary>
/// Business Logic:
/// Maintains discoverable Enterprise Assistant plugins.
///
/// The registry acts as a central catalog of available
/// platform capabilities and enables future orchestration
/// scenarios based on plugin metadata.
/// </summary>
public sealed class PluginRegistry
{
    private readonly IList<IPlugin> _registeredPlugins =
        new List<IPlugin>();

    /// <summary>
    /// Gets the registered plugin definitions.
    /// </summary>
    public IReadOnlyCollection<IPlugin> RegisteredPlugins =>
        _registeredPlugins.ToArray();

    /// <summary>
    /// Business Logic:
    /// Registers a plugin instance in the application
    /// registry and makes it available for discovery.
    /// </summary>
    public void RegisterPlugin(IPlugin plugin)
    {
        if (plugin is null)
        {
            throw new ArgumentNullException(
                nameof(plugin));
        }

        _registeredPlugins.Add(plugin);
    }

    /// <summary>
    /// Business Logic:
    /// Returns all registered plugins.
    /// Used by orchestration and discovery components.
    /// </summary>
    public IReadOnlyCollection<IPlugin> GetPlugins()
    {
        return RegisteredPlugins;
    }

    /// <summary>
    /// Business Logic:
    /// Finds the first plugin capable of handling
    /// the supplied keyword.
    ///
    /// Keyword matching is case-insensitive.
    /// </summary>
    public IPlugin? FindPlugin(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return null;
        }

        return _registeredPlugins.FirstOrDefault(
            plugin =>
                plugin.Keywords.Any(
                    registeredKeyword =>
                        registeredKeyword.Equals(
                            keyword,
                            StringComparison.OrdinalIgnoreCase)));
    }

    /// <summary>
    /// Business Logic:
    /// Returns the names of all registered plugins.
    /// Useful for diagnostics, monitoring,
    /// dashboards, and orchestration.
    /// </summary>
    public IReadOnlyCollection<string> GetPluginNames()
    {
        return _registeredPlugins
            .Select(plugin => plugin.Name)
            .ToArray();
    }

    /// <summary>
    /// Business Logic:
    /// Retrieves a registered plugin by name.
    ///
    /// Used by the Assistant Orchestrator to
    /// dynamically obtain plugin metadata and
    /// capabilities after discovery has occurred.
    /// </summary>
    public IPlugin? GetPlugin(
        string pluginName)
    {
        return _registeredPlugins
            .FirstOrDefault(
                plugin =>
                    plugin.Name.Equals(
                        pluginName,
                        StringComparison.OrdinalIgnoreCase));
    }
}