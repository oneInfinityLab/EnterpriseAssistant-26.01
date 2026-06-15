namespace EnterpriseAssistant.Infrastructure.Configuration;

/// <summary>
/// Configuration options for Azure OpenAI integration.
/// </summary>
public sealed class AzureOpenAIOptions
{
    public string Endpoint { get; init; } = string.Empty;
    public string ApiKey { get; init; } = string.Empty;
    public string DeploymentName { get; init; } = string.Empty;
}
