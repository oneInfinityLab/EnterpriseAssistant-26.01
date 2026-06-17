namespace EnterpriseAssistant.Core.Interfaces;

using EnterpriseAssistant.Core.Models;

public interface IAssistantOrchestrator
{
    ChatResponse ProcessMessage(string message);
}
