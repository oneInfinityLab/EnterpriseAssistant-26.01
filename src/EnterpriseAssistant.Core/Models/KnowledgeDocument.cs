namespace EnterpriseAssistant.Core.Models;

/// <summary>
/// Represents a single document in the enterprise knowledge base.
/// </summary>
public sealed class KnowledgeDocument
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
}
