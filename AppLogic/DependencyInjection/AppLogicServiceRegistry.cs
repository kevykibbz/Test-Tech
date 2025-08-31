using AppLogic.Contracts;
using AppLogic.Ollama.Clients;
using AppLogic.Ollama.Interfaces;
using AppLogic.Ollama.Models;
using DataAccess.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppLogic.DependencyInjection;

public static class AppLogicServiceRegistry
{
    public static IServiceCollection AddAppLogicServices(this IServiceCollection services, IConfiguration configuration, string? ollamaEndpoint = null)
    {
        services.AddDataAccessServices(configuration);

        // Register AppLogic services
        services.AddScoped<ILegalLogic, LegalLogic>();
        services.AddScoped<ILawyerLogic, LawyerLogic>();
        services.AddScoped<IObjectMapper, ObjectMapper>();
        services.AddScoped<IContractService, ContractService>();
        services.AddScoped<ICurrencyLogic, CurrencyLogic>();
        services.AddScoped<IEventLogic, EventLogic>();
        services.AddScoped<IPeopleLogic, PeopleLogic>();
        services.AddScoped<ILogLogic, LogLogic>();
        services.AddScoped<ILegalMatterCategoryLogic, LegalMatterCategoryLogic>();

        services.AddScoped<IContractExtractionService, ContractExtractionService>();

        services.Configure<OllamaConfiguration>(options =>
        {
            // Default configuration - can be overridden by appsettings.json
        });

        services.AddHttpClient<IOllamaClient, OllamaClient>(client =>
        {
            if (!string.IsNullOrEmpty(ollamaEndpoint))
            {
                client.BaseAddress = new Uri(ollamaEndpoint);
            }
            client.Timeout = TimeSpan.FromMinutes(5); // Allow longer timeouts for LLM responses
        });

        return services;
    }
}
