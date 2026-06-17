namespace EnterpriseAssistant.Web.Services;

using System.Collections.Generic;
using EnterpriseAssistant.Core.Models;

/// <summary>
/// Registry of available plugins and knowledge providers.
/// </summary>
public sealed class PluginRegistry
{
    private readonly IList<PluginResult> _registeredPlugins = new List<PluginResult>();

    /// <summary>
    /// Gets the registered plugin definitions.
    /// </summary>
    public IReadOnlyCollection<PluginResult> RegisteredPlugins => (IReadOnlyCollection<PluginResult>)_registeredPlugins;

    /// <summary>
    /// Registers a plugin result placeholder.
    /// </summary>
    public void RegisterPlugin(PluginResult pluginResult)
    {
        if (pluginResult is null)
        {
            throw new ArgumentNullException(nameof(pluginResult));
        }

        _registeredPlugins.Add(pluginResult);
    }
}
