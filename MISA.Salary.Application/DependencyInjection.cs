using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MISA.Salary.Application.UseCases.Implements;
using MISA.Salary.Application.UseCases.Interfaces;

namespace MISA.Salary.Application;
public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplicationLayer(this IServiceCollection services)
    {
        services.AddControllers();

        return services.ConfigureFluentValidation().ConfigureServices();
    }

    public static IServiceCollection ConfigureFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.AddFluentValidationAutoValidation(options =>
        {
            options.DisableDataAnnotationsValidation = true;
        });
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ISalaryCompostionService, SalaryCompositionService>();
        services.AddScoped<ISalaryCompositionSystemService, SalaryCompositionSystemService>();
        services.AddScoped<IOrganizationUnitService, OrganizationUnitService>();
        return services;
    }
}
