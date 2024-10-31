using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Filer.TelegramBot.Presentation.Services;

public class UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger) : IUpdateHandler
{
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
            _                                               => UnknownUpdateHandlerAsync(update)
        });
    }

    private async Task OnMessage(Message msg)
    {
        logger.LogInformation("Receive message type: {MessageType}", msg.Type);
        if (msg.Text is not { } messageText)
        {
            return;
        }

        Message sentMessage = await (messageText.Split(' ')[0] switch
        {
            "/start" => AnswerToStartMessage(msg),
            "/test" => bot.SendTextMessageAsync(msg.Chat, "Test command is working!"),
            "/test2" => bot.SendTextMessageAsync(msg.Chat, "Test 2 command is working!"),
            "/exit" => bot.SendTextMessageAsync(msg.Chat, "Bye!"),
            _ => AnswerToUnknownMessage(msg)
        });
        logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);
    }

    private async Task<Message> AnswerToStartMessage(Message msg)
    {
        const string answer = "Это бот для управления файлами.";
        
        return await bot.SendTextMessageAsync(
            msg.Chat,
            answer,
            parseMode: ParseMode.Html,
            replyMarkup: new ReplyKeyboardRemove());

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
}
