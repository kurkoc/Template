namespace TemplateSolution.Infrastructure.Authentication;

public class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }

    /// <summary>
    ///     dakika cinsinden token expire süresi
    /// </summary>
    public int Expires { get; set; } = 30;
}