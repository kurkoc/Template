using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Minio;

using TemplateSolution.Infrastructure.Storage.Abstractions;
using TemplateSolution.Infrastructure.Storage.Concretes;
using TemplateSolution.Infrastructure.Storage.Types;

namespace TemplateSolution.Infrastructure.Storage;

public static class ServiceCollectionExtensions
{
    public static void AddMinioStorageService(this IServiceCollection services, MinioSettings minioSettings)
    {
        services.AddMinio(configureClient => configureClient
            .WithEndpoint(minioSettings.Url)
            .WithSSL(minioSettings.IsSSL)
            .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey));

        services.AddScoped<IStorageService, MinioStorageService>();
    }

    public static void AddMinioStorageService(this IServiceCollection services, Action<MinioSettings> action)
    {
        MinioSettings minioSettings = new MinioSettings();
        action.Invoke(minioSettings);

        AddMinioStorageService(services, minioSettings);
    }

    public static void AddMinioStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        var minioSettings = configuration.GetSection("MinioSettings").Get<MinioSettings>()!;
        AddMinioStorageService(services, minioSettings);
    }
}