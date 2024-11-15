using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TemplateSolution.Infrastructure.Cors;

public static class ServiceCollectionExtensions
{
    private const string DevPolicy = nameof(DevPolicy);
    private const string CorsPolicy = nameof(CorsPolicy);

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
    {
        CorsSettings? corsSettings = config.GetSection(nameof(CorsSettings)).Get<CorsSettings>();

        return services.AddCors(options =>
        {
            options.AddPolicy(name: DevPolicy,
                policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

            if (corsSettings is not null && corsSettings.AllowedOrigins.Count > 0)
            {
                options.AddPolicy(name: CorsPolicy,
                    policy =>
                    {
                        policy
                            .WithOrigins(corsSettings.AllowedOrigins.ToArray())
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            }
        });
    }
    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
    {
        IHostEnvironment hostEnvironment = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
        if (hostEnvironment.IsDevelopment())
        {
            return app.UseCors(DevPolicy);
        }
        else
        {
            return app.UseCors(CorsPolicy);
        }
    }
}