using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TemplateSolution.Application;

public static  class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutomapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }


    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddAutomapper()
            .AddValidation();
}
