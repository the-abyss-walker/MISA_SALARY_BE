using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MISA.Salary.Domain.Repositories;
using MISA.Salary.Infrastructure.Persistence.Repositories;
using MySqlConnector;

namespace MISA.Salary.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration config)
    {
        return services
            .ConfigureDatabase(config)
            .ConfigureRepositories()
            .ConfigeTypeHandler();
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration config)
    {
        return services.AddMySqlDataSource(
            config.GetConnectionString("Default")
            ?? throw new ApplicationException("Thiếu connection string!!!"));

    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection service)
    {
        service.AddSingleton<IEntityAttributeValues, EntityAttributeValues>();
        service.AddScoped<ISalaryCompositionRepository, SalaryCompositionRepository>();
        service.AddScoped<ISalaryCompositionSystemRepository, SalaryCompositionSystemRepository>();

        return service;
    }

    public static IServiceCollection ConfigeTypeHandler(this IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new JsonListStringHandler());
        return services;
    }
}