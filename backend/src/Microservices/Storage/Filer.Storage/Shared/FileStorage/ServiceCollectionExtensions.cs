using Minio;

namespace Filer.Storage.Shared.FileStorage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterFileStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMinio(configureClient =>
            configureClient.WithEndpoint(configuration["Minio:Endpoint"])
                .WithCredentials(configuration["Minio:AccessKey"], configuration["Minio:SecretKey"])
                .WithSSL(false)
                .Build());

        return services;
    }
}