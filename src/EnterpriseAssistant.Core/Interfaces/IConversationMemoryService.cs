namespace EnterpriseAssistant.Core.Interfaces;

using EnterpriseAssistant.Core.Models;
using System.Threading.Tasks;

public interface IConversationMemoryService
{
    Task AddMessageAsync(string sessionId, ConversationMessage message);
    Task<ConversationContext?> GetConversationAsync(string sessionId);
    Task ClearConversationAsync(string sessionId);
}
