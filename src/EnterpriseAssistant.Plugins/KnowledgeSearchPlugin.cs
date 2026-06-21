namespace EnterpriseAssistant.Plugins;

using EnterpriseAssistant.Core.Interfaces;
using EnterpriseAssistant.Core.Models;
using System.Threading.Tasks;

/// <summary>
/// Business Logic: Plugin for searching enterprise knowledge base.
/// Delegates to IKnowledgeProvider to retrieve relevant documents based on user queries.
/// </summary>
public sealed class KnowledgeSearchPlugin : IPlugin
{
    private readonly IKnowledgeProvider _knowledgeProvider;

    public string Name => "KnowledgeSearchPlugin";

    public string Description =>
    "Searches enterprise knowledge sources.";

    public IReadOnlyList<string> Keywords =>
    [
        "knowledge",
    "search",
    "document",
    "wiki",
    "policy"
    ];

    public KnowledgeSearchPlugin(IKnowledgeProvider knowledgeProvider)
    {
        _knowledgeProvider = knowledgeProvider;
    }

    /// <summary>
    /// Business Logic: Search enterprise knowledge base for documents matching the query.
    /// Returns a KnowledgeSearchResult containing all matching documents.
    /// </summary>
    public async Task<KnowledgeSearchResult> SearchKnowledge(string query)
    {
        // Business Logic: Delegate the search to the knowledge provider.
        // The provider handles the actual search logic (e.g., keyword matching, ranking).
        return await _knowledgeProvider.SearchAsync(query);
    }
}
