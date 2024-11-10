using Filer.TelegramBot.Presentation.Abstract;

namespace Filer.TelegramBot.Presentation.Telegram;

public sealed class PollingService(
    IServiceProvider serviceProvider,
    ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);
