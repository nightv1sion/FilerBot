using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Filer.TelegramBot.Presentation.Persistence;
using Filer.TelegramBot.Presentation.Telegram.Keyboard;
using Filer.TelegramBot.Presentation.UserStates.Callbacks;
using Filer.TelegramBot.Presentation.UserStates;
using Filer.TelegramBot.Presentation.UserStates.Workflows;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Filer.TelegramBot.Presentation.Telegram;

internal sealed class UpdateHandler(
    ITelegramBotClient bot,
    IStorageApi storageApi,
    ApplicationDbContext dbContext,
    WorkflowSerializer workflowSerializer,
    CallbackSerializer callbackSerializer,
    IServiceProvider serviceProvider,
    DirectoryKeyboardPresenter directoryKeyboardPresenter,
    ILogger<UpdateHandler> logger) : IUpdateHandler
{
    private static class Menu
    {
        public const string Storage = "Хранилище";
    }
    
    public async Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        if (exception is RequestException)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
        
        logger.LogError("{Source} RequestException: {Exception}", source, exception);
    }

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        await (update switch
        {
            { Message: { } message }                        => OnMessage(message, cancellationToken),
            { CallbackQuery: {} callbackQuery}              => OnCallbackQuery(callbackQuery, cancellationToken),
            _                                               => UnknownUpdateHandlerAsync(update)
        });
    }

    private async Task OnMessage(Message msg, CancellationToken cancellationToken)
    {
        logger.LogInformation("{UserId} Receive message type: {MessageType}", msg.Chat.Id, msg.Type);
        
        if (msg.Text is null && msg.Document is null)
        {
            return;
        }

        Message? sentMessage = await (msg.Text switch
        {
            "/start" => AnswerToStartMessage(msg),
            Menu.Storage => HandleStorageTab(msg, cancellationToken),
            _ => AnswerToUnknownMessage(msg, cancellationToken)
        });

        if (sentMessage is not null)
        {
            logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);
        }
    }

    private async Task<Message?> AnswerToStartMessage(Message msg)
    {
        if (!await dbContext.UserStates.AnyAsync(x => x.UserId == msg.Chat.Id.ToString()))
        {
            await dbContext.UserStates.AddAsync(new UserState
            {
                Id = Guid.NewGuid(),
                UserId = msg.Chat.Id.ToString()
            }, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }
        
        const string answer = "Это бот для управления файлами";
        
        return await bot.SendTextMessageAsync(
            msg.Chat,
            answer,
            parseMode: ParseMode.Html,
            replyMarkup: new ReplyKeyboardMarkup().AddButton(Menu.Storage));
    }

    private async Task<Message?> AnswerToUnknownMessage(Message msg, CancellationToken cancellationToken)
    {
        var userState = await dbContext.UserStates
            .Include(x => x.CurrentWorkflow)
            .FirstOrDefaultAsync(x => x.UserId == msg.Chat.Id.ToString(), cancellationToken);
        
        if (userState is null)
        {
            throw new InvalidOperationException("User state not found");
        }

        if (userState.CurrentWorkflow is null)
        {
            return await bot.SendTextMessageAsync(
                msg.Chat,
                "Не понимаю о чём речь.",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }

        var currentWorkflow = workflowSerializer.Deserialize(userState.CurrentWorkflow.WorkflowPayload);
        await currentWorkflow.Continue(serviceProvider, msg, cancellationToken);
        return null;
    }
    
    private Task UnknownUpdateHandlerAsync(Update update)
    {
        logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    private async Task<Message?> HandleStorageTab(Message message, CancellationToken cancellationToken)
    {
        var getDirectoryResponse = await storageApi.GetDirectory(
            message.Chat.Id.ToString(),
            null,
            cancellationToken);
        
        DirectoryKeyboardPresenter.Result result = directoryKeyboardPresenter.OpenRootDirectory(
            getDirectoryResponse.SubDirectories
                .Select(x => new DirectoryKeyboardPresenter.DirectoryButton(x.Id, x.Name))
                .ToArray(), 
            getDirectoryResponse.Files
                .Select(x => new DirectoryKeyboardPresenter.FileButton(x.Id, x.Name))
                .ToArray(),
            message.Chat.Id.ToString());
        
        await dbContext.UserCallbacks.AddRangeAsync(result.UserCallbacks, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return await bot.SendTextMessageAsync(
            message.Chat, 
            "Ваше хранилище",
            replyMarkup: result.Keyboard,
            cancellationToken: cancellationToken);
    }

    private async Task OnCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var userCallback = await dbContext.UserCallbacks
            .FirstOrDefaultAsync(x => x.Id.ToString() == callbackQuery.Data, cancellationToken);
        
        if (userCallback is null)
        {
            throw new InvalidOperationException("User callback not found");
        }
        
        ICallback callback = callbackSerializer.Deserialize(userCallback.CallbackPayload);
        await callback.Handle(serviceProvider, callbackQuery, cancellationToken);
    }
}
