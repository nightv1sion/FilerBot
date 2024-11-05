using Filer.TelegramBot.Presentation.Abstract;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Filer.TelegramBot.Presentation.Services;

public sealed class ReceiverService(
    ITelegramBotClient botClient,
    UpdateHandler updateHandler,
    ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);
