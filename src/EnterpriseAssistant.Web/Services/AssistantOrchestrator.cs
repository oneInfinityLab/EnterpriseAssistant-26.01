namespace EnterpriseAssistant.Web.Services;

using Azure;
using EnterpriseAssistant.Core.Interfaces;
using EnterpriseAssistant.Core.Models;
using EnterpriseAssistant.Infrastructure.AI;
using EnterpriseAssistant.Infrastructure.Configuration;
using EnterpriseAssistant.Plugins;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using System;
using System.Linq;

/// <summary>
/// Business Logic: Central orchestrator that manages the chat flow by coordinating
/// Semantic Kernel, plugin invocation, and knowledge retrieval.
/// Acts as the primary entry point for all assistant processing logic.
/// </summary>
public sealed class AssistantOrchestrator : IAssistantOrchestrator
{
    private readonly KernelFactory _kernelFactory;
    private readonly AzureOpenAIOptions _azureOpenAIOptions;
    private readonly KnowledgeSearchPlugin _knowledgeSearchPlugin;
    private readonly IConversationMemoryService _conversationMemoryService;
    private readonly string _sessionId;
    private readonly PluginRegistry _pluginRegistry;

    // Business Logic: Define keywords that trigger knowledge search.
    // These keywords match enterprise document categories and operations.
    private static readonly string[] KnowledgeSearchKeywords =
    {
        "issue",
        "poc",
        "weekend",
        "azure vm",
        "ariba"
    };

    public AssistantOrchestrator(
        KernelFactory kernelFactory,
        IOptions<AzureOpenAIOptions> azureOpenAIOptions,
        KnowledgeSearchPlugin knowledgeSearchPlugin,
        IConversationMemoryService conversationMemoryService, PluginRegistry pluginRegistry)
    {
        _kernelFactory = kernelFactory;
        _azureOpenAIOptions = azureOpenAIOptions.Value;
        _knowledgeSearchPlugin = knowledgeSearchPlugin;
        _conversationMemoryService = conversationMemoryService;
        _pluginRegistry = pluginRegistry;

        // Business Logic: Generate a unique session ID for this orchestrator instance.
        // This enables conversation memory tracking across multiple message exchanges.
        _sessionId = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Business Logic: Process an incoming message through the assistant pipeline.
    /// Validates Azure OpenAI configuration, checks for knowledge search triggers,
    /// and invokes appropriate response path (knowledge search or Semantic Kernel).
    /// Stores all messages (user and assistant) in conversation memory.
    /// 
    /// Execution Flow:
    /// 1. Store user message in conversation memory
    /// 2. Validate Azure OpenAI configuration availability
    /// 3. Check if message contains knowledge search keywords
    /// 4. If keyword matched: invoke knowledge search and return results
    /// 5. If no keyword matched: invoke chat completion via Semantic Kernel
    /// 6. Store assistant response in conversation memory
    /// 
    /// Keywords that trigger knowledge search: "issue", "poc", "weekend", "azure vm", "ariba"
    /// </summary>
    public ChatResponse ProcessMessage(string message)
    {
        // Business Logic: Store the incoming user message in conversation memory.
        // This preserves the conversation history for future context and analysis.
        StoreUserMessageAsync(message).GetAwaiter().GetResult();

        // Business Logic:
        // If Azure OpenAI is not configured, continue operating
        // in local demo mode instead of failing.

        var demoMode =
            string.IsNullOrWhiteSpace(_azureOpenAIOptions.Endpoint) ||
            string.IsNullOrWhiteSpace(_azureOpenAIOptions.DeploymentName);

        try
        {
            var discoveredPlugin = DiscoverPlugin(message);
            // Business Logic:
            // Attempt plugin routing before
            // falling back to knowledge search.
            var routedResponse = AttemptPluginRouting(message);

            if (routedResponse != null)
            {
                StoreAssistantMessageAsync(
                    routedResponse.Message)
                    .GetAwaiter()
                    .GetResult();

                return routedResponse;
            }
            // Business Logic: Attempt knowledge search first.
            // If user message contains enterprise operation keywords, search the knowledge base
            // before invoking the LLM. This provides faster, more accurate responses for known topics.
            var knowledgeResponse = AttemptKnowledgeSearch(message);
            if (knowledgeResponse != null)
            {
                // Business Logic: Store assistant response from knowledge search in memory.
                StoreAssistantMessageAsync(knowledgeResponse.Message).GetAwaiter().GetResult();
                return knowledgeResponse;
            }

            string response;

            if (demoMode)
            {
                response = GetDemoResponse(message);
            }
            else
            {
                var kernel = _kernelFactory.CreateKernel();
                response = InvokeKernelChatCompletion(kernel, message);
            }

            // Business Logic: Store assistant response in conversation memory.
            StoreAssistantMessageAsync(response).GetAwaiter().GetResult();

            return new ChatResponse
            {
                Success = true,
                Message = response
            };
        }
        catch (Exception ex)
        {
            // Business Logic: Capture any execution failures with diagnostic information.
            var errorMessage = $"Error processing message: {ex.Message}";
            StoreAssistantMessageAsync(errorMessage).GetAwaiter().GetResult();

            return new ChatResponse
            {
                Success = false,
                Message = errorMessage
            };
        }
    }
    /// <summary>
    /// Business Logic:
    /// Local Demo Mode responses when Azure OpenAI
    /// is unavailable.
    /// </summary>
    private string GetDemoResponse(string message)
    {
        var discoveredPlugin = DiscoverPlugin(message);

        if (!string.IsNullOrWhiteSpace(discoveredPlugin))
        {
            return
                $"Plugin Discovery: {discoveredPlugin} selected.";
        }
        var lower = message.ToLowerInvariant();

        if (lower.Contains("issue"))
        {
            return "Issue workflow detected. Use the Raise Issue module to create a support request.";
        }

        if (lower.Contains("poc"))
        {
            return "POC workflow detected. Use the Request POC module.";
        }

        if (lower.Contains("weekend"))
        {
            return "Weekend exclusion workflow detected.";
        }

        if (lower.Contains("azure"))
        {
            return "Azure VM workflow detected.";
        }

        if (lower.Contains("ariba"))
        {
            return "Ariba operation workflow detected.";
        }

        return @"Hello Dikshant 👋

Available capabilities:

• Knowledge Search
• Raise Issue
• Request POC
• Weekend Exclusions
• Azure VM Actions
• Ariba Operations

Azure OpenAI is currently running in Demo Mode.";
    }
    /// <summary>
    /// Business Logic: Store a user message in the conversation memory.
    /// The message is timestamped and marked with the 'user' role.
    /// </summary>
    private async System.Threading.Tasks.Task StoreUserMessageAsync(string content)
    {
        var userMessage = new ConversationMessage
        {
            Role = "user",
            Content = content,
            Timestamp = DateTime.UtcNow
        };

        await _conversationMemoryService.AddMessageAsync(_sessionId, userMessage);
    }

    /// <summary>
    /// Business Logic: Store an assistant message in the conversation memory.
    /// The message is timestamped and marked with the 'assistant' role.
    /// </summary>
    private async System.Threading.Tasks.Task StoreAssistantMessageAsync(string content)
    {
        var assistantMessage = new ConversationMessage
        {
            Role = "assistant",
            Content = content,
            Timestamp = DateTime.UtcNow
        };

        await _conversationMemoryService.AddMessageAsync(_sessionId, assistantMessage);
    }

    /// <summary>
    /// Business Logic: Attempt to find matching knowledge documents based on user query.
    /// Checks if the message contains predefined keywords that indicate knowledge search intent.
    /// Returns null if no knowledge match is found, allowing fallback to Semantic Kernel.
    /// </summary>
    private ChatResponse? AttemptKnowledgeSearch(string message)
    {
        // Business Logic: Perform case-insensitive keyword matching against the user message.
        // If any keyword is found, delegate to the knowledge search plugin.
        var lowerMessage = message.ToLowerInvariant();

        var matchedKeyword = KnowledgeSearchKeywords
            .FirstOrDefault(keyword => lowerMessage.Contains(keyword));

        if (string.IsNullOrEmpty(matchedKeyword))
        {
            // Business Logic: No keyword match found, return null to trigger normal assistant response.
            return null;
        }

        // Business Logic: Execute knowledge search using the plugin.
        // Block on async operation for synchronous ProcessMessage contract.
        var searchResult = _knowledgeSearchPlugin.SearchKnowledge(matchedKeyword).Result;

        // Business Logic: Format knowledge search results into a user-friendly response.
        return FormatKnowledgeSearchResponse(searchResult);
    }

    /// <summary>
    /// Business Logic:
    /// Attempts to discover a registered plugin
    /// based on user supplied text.
    /// </summary>
    private string? DiscoverPlugin(string message)
    {
        var words =
            message
                .ToLowerInvariant()
                .Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words)
        {
            var plugin =
                _pluginRegistry.FindPlugin(word);

            if (plugin != null)
            {
                return plugin.Name;
            }
        }

        return null;
    }
    /// <summary>
    /// Business Logic:
    /// Attempts to discover and route execution
    /// to an appropriate enterprise workflow plugin.
    ///
    /// Routing is metadata driven and uses the
    /// Plugin Registry as the source of truth
    /// for plugin capabilities.
    ///
    /// Future versions will invoke Semantic Kernel
    /// function calling and workflow execution.
    /// </summary>
    private ChatResponse? AttemptPluginRouting(
    string message)
    {
        // Business Logic:
        // Normalize user input once so that routing
        // decisions remain case insensitive.

        var normalizedMessage =
            message.ToLowerInvariant();
        var discoveredPlugin =
            DiscoverPlugin(message);

        if (string.IsNullOrWhiteSpace(
            discoveredPlugin))
        {
            return null;
        }

        var plugin =
            _pluginRegistry.GetPlugin(
                discoveredPlugin);

        if (plugin == null)
        {
            return null;
        }
        // Business Logic:
        // Generate a metadata driven plugin response
        // using information provided by the Plugin Registry.
        // This avoids hardcoded plugin knowledge within
        // the Assistant Orchestrator.

        // Business Logic:
        // Execute simple workflow actions before
        // returning plugin metadata.
        //
        // This demonstrates end-to-end plugin execution
        // without requiring workflow forms.

        if (plugin.Name == nameof(IssuePlugin) &&
            normalizedMessage.Contains("create"))
        {
            return new ChatResponse
            {
                Success = true,
                Message =
                    "Issue Plugin execution path detected. " +
                    "Workflow execution support coming in P5.5."
            };
        }

        if (plugin.Name == nameof(PocPlugin) &&
            normalizedMessage.Contains("create"))
        {
            return new ChatResponse
            {
                Success = true,
                Message =
                    "POC Plugin execution path detected. " +
                    "Workflow execution support coming in P5.5."
            };
        }

        if (plugin.Name == nameof(WeekendExclusionPlugin) &&
            normalizedMessage.Contains("create"))
        {
            return new ChatResponse
            {
                Success = true,
                Message =
                    "Weekend Exclusion Plugin execution path detected. " +
                    "Workflow execution support coming in P5.5."
            };
        }

        // Business Logic:
        // If no executable action is detected,
        // return plugin discovery metadata.

        var response =
            $"{plugin.Name} available.\n\n" +
            $"Purpose: {plugin.Description}\n\n" +
            $"Discovery Keywords: {string.Join(", ", plugin.Keywords)}";

        return new ChatResponse
        {
            Success = true,
            Message = response
        };
    }

    /// <summary>
    /// Business Logic:
    /// Executes the selected plugin and returns
    /// an execution result for the assistant.
    /// </summary>
    private string ExecutePlugin(
        string pluginName)
    {
        return pluginName switch
        {
            nameof(IssuePlugin)
                => "Issue Plugin executed successfully.",

            nameof(PocPlugin)
                => "POC Plugin executed successfully.",

            nameof(WeekendExclusionPlugin)
                => "Weekend Exclusion Plugin executed successfully.",

            nameof(KnowledgeSearchPlugin)
                => "Knowledge Search Plugin executed successfully.",

            _ => $"Unknown plugin: {pluginName}"
        };
    }

    /// <summary>
    /// Business Logic: Format knowledge search results into a structured chat response.
    /// Presents matching documents with their titles, categories, and summaries.
    /// </summary>
    private ChatResponse FormatKnowledgeSearchResponse(KnowledgeSearchResult searchResult)
    {
        // Business Logic: Check if the search returned any matching documents.
        var documentsList = searchResult.Results.ToList();

        if (!documentsList.Any())
        {
            // Business Logic: No documents found for the query, return null to fall back to Semantic Kernel.
            return new ChatResponse
            {
                Success = true,
                Message = $"No knowledge documents found for: {searchResult.Query}"
            };
        }

        // Business Logic: Build a formatted response listing all matching documents.
        // Include document title, category, and excerpt from content.
        var formattedResponse = $"Found {documentsList.Count} knowledge document(s) for '{searchResult.Query}':\n\n";

        foreach (var doc in documentsList)
        {
            formattedResponse += $"📄 {doc.Title}\n";
            formattedResponse += $"   Category: {doc.Category}\n";
            formattedResponse += $"   {doc.Content}\n\n";
        }

        return new ChatResponse
        {
            Success = true,
            Message = formattedResponse
        };
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
