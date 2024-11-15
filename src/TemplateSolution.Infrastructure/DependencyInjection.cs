using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateSolution.Infrastructure.Persistence;
using TemplateSolution.Infrastructure.Configuration;
#if (EnableJwt)
using TemplateSolution.Infrastructure.Authentication;
#endif
#if (EnableCors)
using TemplateSolution.Infrastructure.Cors;
#endif
#if (EnableOpenApi)
using TemplateSolution.Infrastructure.OpenApi;
#endif
#if (EnableMinio)
using TemplateSolution.Infrastructure.Storage;
#endif
namespace TemplateSolution.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddConfigurations(configuration);

#if (EnableJwt)
services.AddJwtToken(configuration);
#endif
        
#if (EnableMinio)
services.AddMinioStorageService(configuration);
#endif

#if (EnableOpenApi)
services.AddOpenApiDocument();
#endif

#if (EnableCors)
services.AddCorsPolicy(configuration);
#endif
        
        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        
#if (EnableCors)
app.UseCorsPolicy(configuration);
#endif

#if (EnableJwt)
app.UseJwt();
#endif

#if (EnableOpenApi)
app.UseOpenApiSwagger();
#endif
        return app;
    }
}