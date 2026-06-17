namespace EnterpriseAssistant.Core.Models;

using System;

/// <summary>
/// Represents a single message in a conversation.
/// </summary>
public sealed class ConversationMessage
{
    public string Role { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}
