namespace AppLogic.Ollama.Interfaces;

public interface IOllamaClient
{
    /// <summary>
    /// Generate text from a prompt using the default model
    /// </summary>
    Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate text from a prompt using a specific model
    /// </summary>
    Task<string> GenerateAsync(string prompt, string model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a chat message using the default model
    /// </summary>
    Task<string> ChatAsync(string message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a chat message using a specific model
    /// </summary>
    Task<string> ChatAsync(string message, string model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get health status of the Ollama service
    /// </summary>
    Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default);
}
