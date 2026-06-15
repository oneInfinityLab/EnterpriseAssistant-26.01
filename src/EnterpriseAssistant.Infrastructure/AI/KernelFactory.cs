namespace EnterpriseAssistant.Infrastructure.AI;

using System.Net.Http;
using EnterpriseAssistant.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

/// <summary>
/// Business Logic: Factory responsible for creating and configuring Semantic Kernel instances.
/// Handles Azure OpenAI service initialization and plugin registration.
/// Serves as the single point of kernel creation with consistent configuration.
/// </summary>
public sealed class KernelFactory
{
    private readonly AzureOpenAIOptions _options;

    public KernelFactory(IOptions<AzureOpenAIOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Business Logic: Create a new Semantic Kernel instance with configured services.
    /// Initializes Azure OpenAI chat completion service if properly configured.
    /// Prepares kernel for plugin registration and tool calling.
    /// </summary>
    public Kernel CreateKernel()
    {
        var builder = Kernel.CreateBuilder();

        // Business Logic: Register Azure OpenAI chat completion service if endpoint is configured.
        // The kernel requires a chat completion service to process user messages.
        if (!string.IsNullOrWhiteSpace(_options.Endpoint) &&
            !string.IsNullOrWhiteSpace(_options.DeploymentName))
        {
            builder.AddAzureOpenAIChatCompletion(
                _options.DeploymentName,
                _options.Endpoint,
                _options.ApiKey,
                string.Empty,
                string.Empty,
                new HttpClient(),
                string.Empty);
        }

        // Business Logic: Build kernel with registered services.
        // The kernel is now ready for plugin registration and invocation.
        // Future commits will integrate plugin tool calling via this instance.
        var kernel = builder.Build();

        return kernel;
    }
}
