using TemplateSolution.Infrastructure.Configuration;

namespace TemplateSolution.Infrastructure.Authentication.Types;

public class JwtSettings : IAppSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }

    /// <summary>
    ///     dakika cinsinden token expire süresi
    /// </summary>
    public int Expires { get; set; } = 30;
}