using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Refit;

namespace Filer.TelegramBot.Presentation.ApiClients;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRefitClients(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection
            .AddRefitClient<IStorageApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(configuration["Integration:Storage:Uri"]!);
            });

        return serviceCollection;
    }
}