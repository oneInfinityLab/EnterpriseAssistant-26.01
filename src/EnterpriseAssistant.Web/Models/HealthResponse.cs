namespace EnterpriseAssistant.Web.Models;

/// <summary>
/// Simple health status payload returned by the health endpoint.
/// </summary>
public sealed class HealthResponse
{
    public string Status { get; init; } = string.Empty;
}
