namespace EnterpriseAssistant.Infrastructure.Persistence;

using EnterpriseAssistant.Core.Interfaces;
using EnterpriseAssistant.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Business Logic: In-memory implementation of conversation memory service.
/// Stores conversation history for active sessions in application memory.
/// 
/// Each session maintains a chronological list of messages exchanged between user and assistant.
/// Messages are persisted only for the current application runtime.
/// 
/// Future enhancements:
/// - Database persistence (SQL Server, PostgreSQL)
/// - Redis caching for distributed environments
/// - Cosmos DB for global scale
/// - Conversation analytics and reporting
/// 
/// Note: Vector DB and semantic search integration will be handled by separate components.
/// </summary>
public sealed class InMemoryConversationService : IConversationMemoryService
{
    private readonly Dictionary<string, ConversationContext> _conversations = new();

    /// <summary>
    /// Business Logic: Add a message to a conversation session.
    /// If the session does not exist, it is automatically created.
    /// </summary>
    public Task AddMessageAsync(string sessionId, ConversationMessage message)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new ArgumentException("Session ID cannot be empty.", nameof(sessionId));
        }

        if (message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        lock (_conversations)
        {
            // Business Logic: Create conversation context if it doesn't exist.
            if (!_conversations.ContainsKey(sessionId))
            {
                _conversations[sessionId] = new ConversationContext
                {
                    SessionId = sessionId,
                    Messages = new List<ConversationMessage>()
                };
            }

            // Business Logic: Append message to the conversation history.
            _conversations[sessionId].Messages.Add(message);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Business Logic: Retrieve the full conversation context for a session.
    /// Returns null if the session does not exist.
    /// </summary>
    public Task<ConversationContext?> GetConversationAsync(string sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new ArgumentException("Session ID cannot be empty.", nameof(sessionId));
        }

        lock (_conversations)
        {
            if (!_conversations.TryGetValue(sessionId, out var context))
            {
                return Task.FromResult<ConversationContext?>(null);
            }

            // Business Logic: Return a new instance with copied messages to prevent external modifications.
            var copiedMessages = new List<ConversationMessage>(context.Messages);
            return Task.FromResult<ConversationContext?>(new ConversationContext
            {
                SessionId = context.SessionId,
                Messages = copiedMessages
            });
        }
    }

    /// <summary>
    /// Business Logic: Clear all messages for a conversation session.
    /// The session context is removed completely from memory.
    /// </summary>
    public Task ClearConversationAsync(string sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new ArgumentException("Session ID cannot be empty.", nameof(sessionId));
        }

        lock (_conversations)
        {
            _conversations.Remove(sessionId);
        }

        return Task.CompletedTask;
    }
}
