using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Filer.TelegramBot.Presentation.Persistence;
using Filer.TelegramBot.Presentation.Telegram.Keyboard;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public sealed class OpenFileCallback : ICallback
{
    public required Guid FileId { get; init; }
    
    public async Task Handle(IServiceProvider serviceProvider, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var storageApi = serviceProvider.GetRequiredService<IStorageApi>();
        var bot = serviceProvider.GetRequiredService<ITelegramBotClient>();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var fileKeyboardPresenter = serviceProvider.GetRequiredService<FileKeyboardPresenter>();
        
        var userId = callbackQuery.From.Id.ToString();
        
        var getFileResponse = await storageApi.GetFile(
            userId,
            FileId,
            cancellationToken);

        FileKeyboardPresenter.Result keyboardPresentResult = fileKeyboardPresenter.OpenFile(
            new FileKeyboardPresenter.FileInfo(
                getFileResponse.Id,
                getFileResponse.Name,
                getFileResponse.Path,
                getFileResponse.ParentDirectoryId),
            userId);
        
        await dbContext.UserCallbacks.AddRangeAsync(keyboardPresentResult.UserCallbacks, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await bot.SendTextMessageAsync(
            callbackQuery.From.Id, 
            $"\ud83d\udcc4 {getFileResponse.Path}",
            replyMarkup: keyboardPresentResult.Keyboard,
            cancellationToken: cancellationToken);
    }
    
    public static OpenFileCallback Create(Guid fileId)
    {
        return new OpenFileCallback
        {
            FileId = fileId
        };
    }
}