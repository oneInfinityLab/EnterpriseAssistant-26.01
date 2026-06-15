namespace EnterpriseAssistant.Web.Services;

using EnterpriseAssistant.Core.Interfaces;
using EnterpriseAssistant.Core.Models;

public sealed class AssistantOrchestrator : IAssistantOrchestrator
{
    public ChatResponse ProcessMessage(string message)
    {
        return new ChatResponse
        {
            Success = true,
            Message = $"Assistant received: {message}"
        };
    }
}
