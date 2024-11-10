using Filer.TelegramBot.Presentation.Abstract;
using Filer.TelegramBot.Presentation.Telegram;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Filer.TelegramBot.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection RegisterTelegramIntegration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<BotConfiguration>(configuration.GetSection("TelegramIntegration:BotConfiguration"));

        services
            .AddHttpClient("telegram_bot_client")
            .RemoveAllLoggers()
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                BotConfiguration? botConfiguration = sp.GetService<IOptions<BotConfiguration>>()?.Value;
                ArgumentNullException.ThrowIfNull(botConfiguration);
                TelegramBotClientOptions options = new(botConfiguration.BotToken);
                return new TelegramBotClient(options, httpClient);
            });

        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
        services.AddScoped<IMessageSender, MessageSender>();
        return services;
    }
}