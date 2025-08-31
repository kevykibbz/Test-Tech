using AppLogic.Ollama.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AppLogic.Ollama.Extensions;

public class OllamaHealthCheck(IOllamaClient ollamaClient) : IHealthCheck
{
    private readonly IOllamaClient _ollamaClient = ollamaClient;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var isHealthy = await _ollamaClient.IsHealthyAsync(cancellationToken);

            return isHealthy
                ? HealthCheckResult.Healthy("Ollama service is healthy")
                : HealthCheckResult.Unhealthy("Ollama service is not responding");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Ollama service health check failed", ex);
        }
    }
}
