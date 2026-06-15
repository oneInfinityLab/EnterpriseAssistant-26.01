namespace EnterpriseAssistant.Web.Controllers;

using EnterpriseAssistant.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

public sealed class ChatController : ControllerBase
{
    private readonly IAssistantOrchestrator _assistantOrchestrator;

    public ChatController(IAssistantOrchestrator assistantOrchestrator)
    {
        _assistantOrchestrator = assistantOrchestrator;
    }

    [HttpPost("api/chat")]
    public IActionResult Post([FromBody] ChatRequestDto request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new { error = "Message is required." });
        }

        var result = _assistantOrchestrator.ProcessMessage(request.Message);
        return Ok(new { response = result.Message });
    }

    public sealed class ChatRequestDto
    {
        public string Message { get; init; } = string.Empty;
    }
}
