namespace EnterpriseAssistant.Infrastructure.AI;

using EnterpriseAssistant.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

public sealed class AzureOpenAIService
{
    private readonly KernelFactory _kernelFactory;
    private readonly AzureOpenAIOptions _options;

    public AzureOpenAIService(KernelFactory kernelFactory, IOptions<AzureOpenAIOptions> options)
    {
        _kernelFactory = kernelFactory;
        _options = options.Value;
    }

    public Kernel GetKernel()
    {
        return _kernelFactory.CreateKernel();
    }

    public bool IsEnabled()
    {
        return !string.IsNullOrWhiteSpace(_options.Endpoint) &&
               !string.IsNullOrWhiteSpace(_options.DeploymentName) &&
               !string.IsNullOrWhiteSpace(_options.ApiKey);
    }
}
