using Filer.TelegramBot.Presentation.Abstract;
using Telegram.Bot;

namespace Filer.TelegramBot.Presentation.Telegram;

public sealed class ReceiverService(
    ITelegramBotClient botClient,
    UpdateHandler updateHandler,
    ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);
