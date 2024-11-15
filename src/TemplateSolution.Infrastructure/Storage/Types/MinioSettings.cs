using TemplateSolution.Infrastructure.Configuration;

namespace TemplateSolution.Infrastructure.Storage.Types;

public class MinioSettings : IAppSettings
{
    public string Url { get; set; }
    public bool IsSSL { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
}