using TemplateSolution.Infrastructure.Configuration;

namespace TemplateSolution.Infrastructure.Cors;

public class CorsSettings : IAppSettings
{
    public List<string> AllowedOrigins { get; } = [];
}