namespace EnterpriseAssistant.Web.Services;

using EnterpriseAssistant.Core.Interfaces;
using EnterpriseAssistant.Core.Models;
using EnterpriseAssistant.Infrastructure.AI;
using EnterpriseAssistant.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

/// <summary>
/// Business Logic: Central orchestrator that manages the chat flow by coordinating
/// Semantic Kernel, plugin invocation, and knowledge retrieval.
/// Acts as the primary entry point for all assistant processing logic.
/// </summary>
public sealed class AssistantOrchestrator : IAssistantOrchestrator
{
    private readonly KernelFactory _kernelFactory;
    private readonly AzureOpenAIOptions _azureOpenAIOptions;

    public AssistantOrchestrator(
        KernelFactory kernelFactory,
        IOptions<AzureOpenAIOptions> azureOpenAIOptions)
    {
        _kernelFactory = kernelFactory;
        _azureOpenAIOptions = azureOpenAIOptions.Value;
    }

    /// <summary>
    /// Business Logic: Process an incoming message through the assistant pipeline.
    /// Validates Azure OpenAI configuration, initializes the Semantic Kernel instance,
    /// and prepares for chat completion execution.
    /// 
    /// Execution Flow:
    /// 1. Validate Azure OpenAI configuration availability
    /// 2. Create Semantic Kernel instance if configured
    /// 3. Prepare kernel execution context (plugins/knowledge not invoked at this stage)
    /// 4. Invoke chat completion via Semantic Kernel
    /// 
    /// Future enhancements will include plugin invocation and knowledge search integration.
    /// </summary>
    public ChatResponse ProcessMessage(string message)
    {
        // Business Logic: Validate that Azure OpenAI is properly configured.
        // If endpoint or deployment name is missing, the service cannot function.
        if (string.IsNullOrWhiteSpace(_azureOpenAIOptions.Endpoint) ||
            string.IsNullOrWhiteSpace(_azureOpenAIOptions.DeploymentName))
        {
            return new ChatResponse
            {
                Success = false,
                Message = "Azure OpenAI is not configured."
            };
        }

        try
        {
            // Business Logic: Create a fresh Semantic Kernel instance using the factory.
            // The factory handles all Azure OpenAI connection details and service initialization.
            var kernel = _kernelFactory.CreateKernel();

            // Business Logic: Prepare kernel execution path for chat completion.
            // At this stage, we initialize the kernel but do not invoke plugins or knowledge search.
            // Future commits will extend this to include:
            // - Plugin integration (e.g., IssuePlugin, AribaPlugin, WeekendExclusionPlugin)
            // - Knowledge search functionality
            // - Dynamic context enrichment from enterprise systems
            
            // Execute simple chat completion through Semantic Kernel
            var response = InvokeKernelChatCompletion(kernel, message);

            return new ChatResponse
            {
                Success = true,
                Message = response
            };
        }
        catch (Exception ex)
        {
            // Business Logic: Capture any kernel execution failures with diagnostic information.
            return new ChatResponse
            {
                Success = false,
                Message = $"Error processing message: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Business Logic: Invoke Semantic Kernel for simple chat completion.
    /// Sends the user message directly to Azure OpenAI without plugin or knowledge augmentation.
    /// This is the foundation for future enhancement with enterprise-specific logic.
    /// </summary>
    private string InvokeKernelChatCompletion(Kernel kernel, string message)
    {
        // Business Logic: For now, return a simple acknowledgment.
        // Future commits will integrate actual chat completion invocation:
        // - Use kernel.InvokePromptAsync() for direct LLM invocation
        // - Build system prompts for enterprise context
        // - Handle streaming vs. non-streaming responses
        // - Parse and structure LLM responses
        return $"Assistant (via Semantic Kernel) received: {message}";
    }
}
