namespace AppLogic.Ollama.Models;

public class OllamaException : Exception
{
    public string? Model { get; }
    public string? Endpoint { get; }

    public OllamaException(string message) : base(message)
    {
    }

    public OllamaException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public OllamaException(string message, string? model, string? endpoint) : base(message)
    {
        Model = model;
        Endpoint = endpoint;
    }

    public OllamaException(string message, Exception innerException, string? model, string? endpoint) : base(message, innerException)
    {
        Model = model;
        Endpoint = endpoint;
    }
}

public class OllamaModelNotFoundException(string modelName) : OllamaException($"Model '{modelName}' not found or not available") { }

public class OllamaTimeoutException(string operation, TimeSpan timeout) : OllamaException($"Operation '{operation}' timed out after {timeout.TotalSeconds} seconds") { }
