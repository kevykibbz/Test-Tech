using AppLogic.DependencyInjection;

namespace WebApi.DependencyInjection;

public static class WebApiServiceRegistry
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        // Get Ollama endpoint from configuration
        var ollamaEndpoint = configuration["OLLAMA_ENDPOINT"];

        // Register AppLogic services (which includes DataAccess)
        services.AddAppLogicServices(configuration, ollamaEndpoint);

        return services;
    }
}
