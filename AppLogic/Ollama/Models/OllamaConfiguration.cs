namespace AppLogic.Ollama.Models;

public class OllamaConfiguration
{
    public string DefaultModel { get; set; } = "llama3.2:1b";
    public int TimeoutMinutes { get; set; } = 5;
    public int MaxRetries { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 2;
    public bool LogPrompts { get; set; } = true;
    public bool LogResponses { get; set; } // Potentially sensitive data
}
