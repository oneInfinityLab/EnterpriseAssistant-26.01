namespace EnterpriseAssistant.Core.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using EnterpriseAssistant.Core.Models;

/// <summary>
/// Represents a provider that can resolve knowledge or plugin-driven content.
/// </summary>
public interface IKnowledgeProvider
{
    /// <summary>
    /// Searches enterprise knowledge and returns a plugin-style result.
    /// </summary>
    Task<PluginResult> SearchKnowledgeAsync(string query, CancellationToken cancellationToken = default);
}
