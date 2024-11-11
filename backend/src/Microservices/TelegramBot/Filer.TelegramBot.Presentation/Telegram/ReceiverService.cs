using Filer.TelegramBot.Presentation.Abstract;
using Telegram.Bot;

namespace Filer.TelegramBot.Presentation.Telegram;

internal sealed class ReceiverService(
    ITelegramBotClient botClient,
    UpdateHandler updateHandler,
    ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);
