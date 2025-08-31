
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.DependencyInjection;

public static class DataAccessServiceRegistry
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILegalRepository, LegalRepository>();
        services.AddScoped<ILawyerRepository, LawyerRepository>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IPeopleRepository, PeopleRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<ILegalMatterCategoryRepository, LegalMatterCategoryRepository>();

        services.AddDbContext<TechTestDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("LawVuTechTestDB")));

        return services;
    }
}
