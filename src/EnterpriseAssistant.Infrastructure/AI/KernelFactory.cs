namespace EnterpriseAssistant.Infrastructure.AI;

using System.Net.Http;
using EnterpriseAssistant.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

public sealed class KernelFactory
{
    private readonly AzureOpenAIOptions _options;

    public KernelFactory(IOptions<AzureOpenAIOptions> options)
    {
        _options = options.Value;
    }

    public Kernel CreateKernel()
    {
        var builder = Kernel.CreateBuilder();

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

        return builder.Build();
    }
}
