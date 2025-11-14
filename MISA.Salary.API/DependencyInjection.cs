using MISA.Salary.Application;
using MISA.Salary.Infrastructure;

namespace MISA.Salary.API;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureDependencyLayer(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();
        return services
            .ConfigureInfrastructureLayer(config)
            .ConfigureApplicationLayer();
    }
}