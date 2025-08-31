using AppLogic.Ollama.Interfaces;
using AppLogic.Ollama.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace AppLogic.Ollama.Clients;

public class OllamaClient : IOllamaClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OllamaClient> _logger;
    private readonly OllamaConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions;

    // LoggerMessage delegates for better performance
    private static readonly Action<ILogger, string, string, Exception?> _logGenerateRequestWithPrompt =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(1, nameof(GenerateAsync)),
            "Sending generate request to Ollama with model: {Model}, prompt: {Prompt}");

    private static readonly Action<ILogger, string, Exception?> _logGenerateRequest =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(2, nameof(GenerateAsync)),
            "Sending generate request to Ollama with model: {Model}");

    private static readonly Action<ILogger, string, Exception?> _logGenerateResponse =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(3, nameof(GenerateAsync)),
            "Received response from Ollama: {Response}");

    private static readonly Action<ILogger, int, Exception?> _logGenerateResponseLength =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(4, nameof(GenerateAsync)),
            "Received response from Ollama (length: {Length})");

    private static readonly Action<ILogger, string, string, Exception?> _logChatRequestWithMessage =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(5, nameof(ChatAsync)),
            "Sending chat request to Ollama with model: {Model}, message: {Message}");

    private static readonly Action<ILogger, string, Exception?> _logChatRequest =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(6, nameof(ChatAsync)),
            "Sending chat request to Ollama with model: {Model}");

    private static readonly Action<ILogger, string, Exception?> _logChatResponse =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(7, nameof(ChatAsync)),
            "Received chat response from Ollama: {Response}");

    private static readonly Action<ILogger, int, Exception?> _logChatResponseLength =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(8, nameof(ChatAsync)),
            "Received chat response from Ollama (length: {Length})");

    private static readonly Action<ILogger, Exception?> _logHealthCheckFailed =
        LoggerMessage.Define(LogLevel.Error, new EventId(9, nameof(IsHealthyAsync)),
            "Health check failed for Ollama service");

    private static readonly Action<ILogger, int, int, string, double, Exception?> _logRetryWarning =
        LoggerMessage.Define<int, int, string, double>(LogLevel.Warning, new EventId(10, "Retry"),
            "Attempt {Attempt}/{MaxRetries} failed for {Operation}. Retrying in {Delay}s...");

    private static readonly Action<ILogger, string, int, Exception?> _logOperationFailed =
        LoggerMessage.Define<string, int>(LogLevel.Error, new EventId(11, "OperationFailed"),
            "Operation {Operation} failed after {Attempt} attempts");

    public OllamaClient(HttpClient httpClient, ILogger<OllamaClient> logger, IOptions<OllamaConfiguration> configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration.Value;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default)
    {
        return await GenerateAsync(prompt, _configuration.DefaultModel, cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> GenerateAsync(string prompt, string model, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        return await ExecuteWithRetryAsync(async () =>
        {
            var request = new OllamaGenerateRequest(prompt, model);

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            if (_configuration.LogPrompts)
            {
                _logGenerateRequestWithPrompt(_logger, model, prompt, null);
            }
            else
            {
                _logGenerateRequest(_logger, model, null);
            }

            var response = await _httpClient.PostAsync(new Uri("/api/generate", UriKind.Relative), content, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new OllamaModelNotFoundException(model);
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseContent, _jsonOptions);

            var result = ollamaResponse?.Response ?? string.Empty;

            if (_configuration.LogResponses)
            {
                _logGenerateResponse(_logger, result, null);
            }
            else
            {
                _logGenerateResponseLength(_logger, result.Length, null);
            }

            return result;
        }, "Generate", cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> ChatAsync(string message, CancellationToken cancellationToken = default)
    {
        return await ChatAsync(message, _configuration.DefaultModel, cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> ChatAsync(string message, string model, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        return await ExecuteWithRetryAsync(async () =>
        {
            var messages = new Collection<OllamaChatMessage> { new OllamaChatMessage { Role = "user", Content = message } };
            var request = new OllamaChatRequest(messages, model);

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            if (_configuration.LogPrompts)
            {
                _logChatRequestWithMessage(_logger, model, message, null);
            }
            else
            {
                _logChatRequest(_logger, model, null);
            }

            var response = await _httpClient.PostAsync(new Uri("/api/chat", UriKind.Relative), content, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new OllamaModelNotFoundException(model);
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var ollamaResponse = JsonSerializer.Deserialize<OllamaChatResponse>(responseContent, _jsonOptions);

            var result = ollamaResponse?.Message?.Content ?? string.Empty;

            if (_configuration.LogResponses)
            {
                _logChatResponse(_logger, result, null);
            }
            else
            {
                _logChatResponseLength(_logger, result.Length, null);
            }

            return result;
        }, "Chat", cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(new Uri("/", UriKind.Relative), cancellationToken).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logHealthCheckFailed(_logger, ex);
            return false;
        }
        catch (TaskCanceledException ex)
        {
            _logHealthCheckFailed(_logger, ex);
            return false;
        }
    }

    private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, string operationName, CancellationToken cancellationToken)
    {
        var maxRetries = _configuration.MaxRetries;
        var retryDelay = TimeSpan.FromSeconds(_configuration.RetryDelaySeconds);

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                throw new OllamaTimeoutException(operationName, TimeSpan.FromMinutes(_configuration.TimeoutMinutes));
            }
            catch (HttpRequestException ex) when (attempt < maxRetries)
            {
                _logRetryWarning(_logger, attempt, maxRetries, operationName, retryDelay.TotalSeconds, ex);

                await Task.Delay(retryDelay, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (attempt < maxRetries && IsRetryableException(ex))
            {
                _logRetryWarning(_logger, attempt, maxRetries, operationName, retryDelay.TotalSeconds, ex);

                await Task.Delay(retryDelay, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logOperationFailed(_logger, operationName, attempt, ex);
                throw new OllamaException($"Failed to execute {operationName} after {attempt} attempts", ex);
            }
        }

        throw new OllamaException($"All {maxRetries} attempts failed for {operationName}");
    }

    private static bool IsRetryableException(Exception ex)
    {
        return ex is HttpRequestException or TaskCanceledException or SocketException or TimeoutException;
    }
}
