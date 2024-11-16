using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TemplateSolution.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        IEnumerable<Type> settingTypes = assembly.GetTypes()
            .Where(type => typeof(IAppSettings).IsAssignableFrom(type) 
                           && type is { IsClass: true, IsAbstract: false });
        
        foreach (Type type in settingTypes)
        {
            IConfigurationSection section = configuration.GetSection(type.Name);
            object? setting = section.Get(type);
            if (setting is null) continue;
            
            services.AddSingleton(type, setting);
            
            MethodInfo? configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethod(nameof(OptionsConfigurationServiceCollectionExtensions.Configure),
                    [typeof(IServiceCollection), typeof(IConfiguration)])?
                .MakeGenericMethod(type);

            configureMethod?.Invoke(null, [services, section]);
        }
    }
}