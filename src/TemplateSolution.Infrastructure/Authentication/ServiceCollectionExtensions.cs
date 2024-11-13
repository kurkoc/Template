using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using TemplateSolution.Infrastructure.Authentication.Abstractions;
using TemplateSolution.Infrastructure.Authentication.Concretes;

namespace TemplateSolution.Infrastructure.Authentication;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtToken(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        if(settings is null) throw new ArgumentNullException(nameof(settings));

        services.AddSingleton<JwtSettings>(settings);
        services.AddHttpContextAccessor();
        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddAuthorization();
        return services;
    }

    public static void UseJwt(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}