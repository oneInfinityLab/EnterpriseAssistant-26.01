namespace EnterpriseAssistant.Core.Models;

using System.Collections.Generic;

/// <summary>
/// Represents the full context of a conversation session.
/// </summary>
public sealed class ConversationContext
{
    public string SessionId { get; init; } = string.Empty;
    public IList<ConversationMessage> Messages { get; init; } = new List<ConversationMessage>();
}
