using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using TemplateSolution.Application.Services;

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

    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<IBusinessService>()
            .AddClasses(classes => classes.AssignableTo<IBusinessService>())
            .AsImplementedInterfaces(type => type != typeof(IBusinessService))
            .WithScopedLifetime());

        return services;
    }


    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddBusinessServices()
            .AddAutomapper()
            .AddValidation();
}
