namespace EnterpriseAssistant.Web.Services;

using EnterpriseAssistant.Core.Models;
using EnterpriseAssistant.Plugins;
using System.Collections.Generic;

/// <summary>
/// Business Logic: Service responsible for plugin discovery and registration.
/// Maintains a registry of available enterprise plugins with their metadata.
/// Provides centralized access to plugin definitions for orchestration and introspection.
/// 
/// Future enhancements:
/// - Dynamic plugin loading from external assemblies
/// - Plugin versioning and compatibility checking
/// - Plugin dependency resolution
/// - Plugin lifecycle management (initialization, cleanup)
/// </summary>
public sealed class PluginRegistrationService
{
    private readonly List<PluginMetadata> _registeredPlugins = new();

    public PluginRegistrationService()
    {
        // Business Logic: Initialize with predefined enterprise plugins.
        // Each plugin is described with metadata for discovery and management.
        RegisterDefaultPlugins();
    }

    /// <summary>
    /// Gets all registered plugin metadata in read-only format.
    /// </summary>
    public IReadOnlyList<PluginMetadata> GetRegisteredPlugins() => _registeredPlugins.AsReadOnly();

    /// <summary>
    /// Gets metadata for a specific plugin by name.
    /// Returns null if the plugin is not found.
    /// </summary>
    public PluginMetadata? GetPluginMetadata(string pluginName)
    {
        return _registeredPlugins.Find(p => p.Name.Equals(pluginName, System.StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Business Logic: Registers a plugin with its metadata.
    /// Supports future dynamic registration of plugins discovered at runtime.
    /// </summary>
    public void RegisterPlugin(PluginMetadata metadata)
    {
        if (metadata is null)
        {
            throw new ArgumentNullException(nameof(metadata));
        }

        if (string.IsNullOrWhiteSpace(metadata.Name))
        {
            throw new ArgumentException("Plugin name cannot be empty.", nameof(metadata));
        }

        // Business Logic: Prevent duplicate plugin registration.
        // Each plugin name must be unique within the registry.
        if (_registeredPlugins.Exists(p => p.Name.Equals(metadata.Name, System.StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Plugin '{metadata.Name}' is already registered.");
        }

        _registeredPlugins.Add(metadata);
    }

    /// <summary>
    /// Business Logic: Initialize registry with built-in enterprise plugins.
    /// These represent the core plugins available in the current release.
    /// </summary>
    private void RegisterDefaultPlugins()
    {
        // Business Logic: Knowledge Search Plugin
        // Provides enterprise knowledge base search capability for user queries.
        // Matches user intent against predefined keywords and returns relevant documents.
        RegisterPlugin(new PluginMetadata
        {
            Name = nameof(KnowledgeSearchPlugin),
            Description = "Search enterprise knowledge base for documents matching user queries",
            Version = "1.0.0",
            IsEnabled = true
        });

        // Business Logic: Issue Plugin (reserved for V2)
        // Will enable users to raise operational issues in enterprise systems.
        RegisterPlugin(new PluginMetadata
        {
            Name = "IssuePlugin",
            Description = "Raise operational issues in enterprise systems",
            Version = "2.0.0",
            IsEnabled = false
        });

        // Business Logic: POC Plugin (reserved for V2)
        // Will allow users to submit Proof of Concept (POC) requests for technology evaluation.
        RegisterPlugin(new PluginMetadata
        {
            Name = "PocPlugin",
            Description = "Submit Proof of Concept (POC) requests for new technology evaluation",
            Version = "2.0.0",
            IsEnabled = false
        });

        // Business Logic: Weekend Exclusion Plugin (reserved for V2)
        // Will manage change exclusion windows during weekends for system stability.
        RegisterPlugin(new PluginMetadata
        {
            Name = "WeekendExclusionPlugin",
            Description = "Manage change exclusion windows during weekends",
            Version = "2.0.0",
            IsEnabled = false
        });

        // Business Logic: Azure VM Plugin (reserved for V2)
        // Will provide Azure Virtual Machine operations and management capabilities.
        RegisterPlugin(new PluginMetadata
        {
            Name = "AzureVmPlugin",
            Description = "Manage Azure Virtual Machine operations",
            Version = "2.0.0",
            IsEnabled = false
        });

        // Business Logic: Ariba Plugin (reserved for V3)
        // Will integrate with Ariba for procurement and supply chain operations.
        RegisterPlugin(new PluginMetadata
        {
            Name = "AribaPlugin",
            Description = "Integrate with Ariba for procurement and supply chain operations",
            Version = "3.0.0",
            IsEnabled = false
        });
    }
}
