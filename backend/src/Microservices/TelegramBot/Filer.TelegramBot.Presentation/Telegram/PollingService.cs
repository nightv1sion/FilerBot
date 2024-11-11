using Filer.TelegramBot.Presentation.Abstract;

namespace Filer.TelegramBot.Presentation.Telegram;

internal sealed class PollingService(
    IServiceProvider serviceProvider,
    ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);
