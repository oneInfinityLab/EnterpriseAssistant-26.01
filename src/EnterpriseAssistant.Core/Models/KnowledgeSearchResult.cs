namespace EnterpriseAssistant.Core.Models;

using System.Collections.Generic;

/// <summary>
/// Represents the result of a knowledge base search query.
/// </summary>
public sealed class KnowledgeSearchResult
{
    public string Query { get; init; } = string.Empty;
    public IEnumerable<KnowledgeDocument> Results { get; init; } = [];
}
