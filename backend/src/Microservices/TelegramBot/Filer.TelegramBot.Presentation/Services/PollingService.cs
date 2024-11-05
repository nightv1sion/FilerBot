using Filer.TelegramBot.Presentation.Abstract;
using Microsoft.Extensions.Logging;

namespace Filer.TelegramBot.Presentation.Services;

public sealed class PollingService(
    IServiceProvider serviceProvider,
    ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);
