using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TemplateSolution.Infrastructure.Persistence.Context;

namespace TemplateSolution.Infrastructure.Persistence;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TemplateSolutionContext>((serviceProvider, contextOptions) =>
        {
            var hostEnvironment = serviceProvider.GetService<IHostEnvironment>();
            var config = serviceProvider.GetService<IConfiguration>();

            contextOptions
                .EnableSensitiveDataLogging(hostEnvironment.IsDevelopment())
                .EnableDetailedErrors(hostEnvironment.IsDevelopment());

#if (UseSqlServer)
                contextOptions.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
#endif
#if (UsePostgresql)
                contextOptions.UseNpgsql(configuration.GetConnectionString("PostgresqlConnection"));
#endif
        });

        return services;
    }
}
