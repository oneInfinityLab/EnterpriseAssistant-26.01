namespace EnterpriseAssistant.Web.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using EnterpriseAssistant.Core.Interfaces;

/// <summary>
/// Registry of available plugins.
/// </summary>
public sealed class PluginRegistry
{
    private readonly IList<IPlugin> _registeredPlugins = new List<IPlugin>();

    /// <summary>
    /// Gets the registered plugin definitions.
    /// </summary>
    public IReadOnlyCollection<IPlugin> RegisteredPlugins => _registeredPlugins.ToArray();

    /// <summary>
    /// Registers a plugin instance in the application registry.
    /// </summary>
    public void RegisterPlugin(IPlugin plugin)
    {
        if (plugin is null)
        {
            throw new ArgumentNullException(nameof(plugin));
        }

        _registeredPlugins.Add(plugin);
    }
}
