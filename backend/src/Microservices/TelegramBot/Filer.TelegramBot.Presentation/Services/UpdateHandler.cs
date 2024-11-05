using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Filer.TelegramBot.Presentation.Services;

public sealed class UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger) : IUpdateHandler
{
    private static class Menu
    {
        public const string MyFiles = "Мои файлы";
    }

    private static class CallbackData
    {
        public const string OpenDirectory = "open_directory";
    }
    
    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        if (exception is RequestException)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message }                        => OnMessage(message),
            { CallbackQuery: {} callbackQuery}              => OnCallbackQuery(callbackQuery),
            _                                               => UnknownUpdateHandlerAsync(update)
        });
    }

    private async Task OnMessage(Message msg)
    {
        logger.LogInformation("{UserId} Receive message type: {MessageType}", msg.Chat.Id, msg.Type);
        if (msg.Text is not { } messageText)
        {
            return;
        }

        Message sentMessage = await (messageText switch
        {
            "/start" => AnswerToStartMessage(msg),
            Menu.MyFiles => HandleMyFiles(msg),
            _ => AnswerToUnknownMessage(msg)
        });
        logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);
    }

    private async Task<Message> AnswerToStartMessage(Message msg)
    {
        const string answer = "Это бот для управления файлами. Меню:";
        
        return await bot.SendTextMessageAsync(
            msg.Chat,
            answer,
            parseMode: ParseMode.Html,
            replyMarkup: new ReplyKeyboardMarkup()
                .AddButton("Мои файлы"));
    }

    private async Task<Message> AnswerToUnknownMessage(Message msg)
    {
        const string answer = "Не понимаю о чём речь.";

        return await bot.SendTextMessageAsync(
            msg.Chat,
            answer,
            parseMode: ParseMode.Html,
            replyMarkup: new ReplyKeyboardRemove());
    }
    
    private Task UnknownUpdateHandlerAsync(Update update)
    {
        logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    private Task<Message> HandleMyFiles(Message message)
    {
        var keyboard = new InlineKeyboardMarkup()
            .AddButton("Папка 1", CallbackData.OpenDirectory)
            .AddButton("Папка 2", CallbackData.OpenDirectory);

        return bot.SendTextMessageAsync(message.Chat, "Выберите папку", replyMarkup: keyboard);
    }

    private Task OnCallbackQuery(CallbackQuery callbackQuery)
    {
        return callbackQuery.Data switch
        {
            CallbackData.OpenDirectory => HandleMyFiles(callbackQuery.Message!),
            _ => bot.SendTextMessageAsync(callbackQuery.ChatInstance, "Не понимаю о чём речь")
        };
    }
}
