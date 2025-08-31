using System.Collections.ObjectModel;

namespace AppLogic.Ollama.Models;

public class OllamaGenerateRequest
{
    // public string Model { get; set; } = "mistral"; // commented out for flexibility
    public string Model { get; set; }
    public OllamaGenerateRequest(string prompt, string model = "llama3.2:1b", bool stream = false)
    {
        Prompt = prompt;
        Model = model;
        Stream = stream;
    }
    public string Prompt { get; set; } = string.Empty;
    public bool Stream { get; set; }
}

public class OllamaChatRequest
{
    // public string Model { get; set; } = "mistral"; // commented out for flexibility
    public string Model { get; set; }
    public Collection<OllamaChatMessage> Messages { get; }
    public bool Stream { get; set; }
    public OllamaChatRequest(Collection<OllamaChatMessage> messages, string model = "llama3.2:1b", bool stream = false)
    {
        Messages = messages;
        Model = model;
        Stream = stream;
    }
}

public class OllamaChatMessage
{
    public string Role { get; set; } = "user";
    public string Content { get; set; } = string.Empty;
}

public class OllamaResponse
{
    public string Response { get; set; } = string.Empty;
    public bool Done { get; set; }
    public string Model { get; set; } = string.Empty;
}

public class OllamaChatResponse
{
    public OllamaChatMessage Message { get; set; } = new();
    public bool Done { get; set; }
    public string Model { get; set; } = string.Empty;
}

public class OllamaModelList
{
    public Collection<OllamaModel> Models { get; }
    public OllamaModelList(Collection<OllamaModel> models)
    {
        Models = models;
    }
}

public class OllamaModel
{
    public string Name { get; set; } = string.Empty;
    public DateTime ModifiedAt { get; set; }
    public long Size { get; set; }
}
