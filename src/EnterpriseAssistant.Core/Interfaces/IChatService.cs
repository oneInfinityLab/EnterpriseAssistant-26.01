namespace EnterpriseAssistant.Core.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using EnterpriseAssistant.Core.Models;

/// <summary>
/// Defines the contract for chat request orchestration.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Sends a chat request and returns a structured response.
    /// </summary>
    Task<ChatResponse> SendChatRequestAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
