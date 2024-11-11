using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Filer.TelegramBot.Presentation.Persistence;
using Filer.TelegramBot.Presentation.Telegram;
using Filer.TelegramBot.Presentation.Telegram.Keyboard;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public sealed class OpenDirectoryCallback : ICallback
{
    public required Guid? DirectoryId { get; init; }
    
    public async Task Handle(
        IServiceProvider serviceProvider,
        CallbackQuery callbackQuery,
        CancellationToken cancellationToken)
    {
        var storageApi = serviceProvider.GetRequiredService<IStorageApi>();
        var bot = serviceProvider.GetRequiredService<ITelegramBotClient>();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var directoryKeyboardPresenter = serviceProvider.GetRequiredService<DirectoryKeyboardPresenter>();
        
        var getDirectoriesResponse = await storageApi.GetDirectories(
            callbackQuery.From.Id.ToString(),
            DirectoryId,
            cancellationToken);

        DirectoryKeyboardPresenter.Result keyboardPresentResult;
        
        if (DirectoryId is null)
        {
            keyboardPresentResult = directoryKeyboardPresenter.OpenRootDirectory(
                getDirectoriesResponse.SubDirectories
                    .Select(x => new DirectoryKeyboardPresenter.DirectoryButton(x.Id, x.Name))
                    .ToArray(), 
                callbackQuery.From.Id.ToString());
        }
        else
        {
            keyboardPresentResult = directoryKeyboardPresenter.OpenDirectory(
                getDirectoriesResponse.SubDirectories
                    .Select(x => new DirectoryKeyboardPresenter.DirectoryButton(x.Id, x.Name))
                    .ToArray(), 
                callbackQuery.From.Id.ToString(),
                DirectoryId.Value,
                getDirectoriesResponse.Directory?.ParentDirectoryId);
        }
        
        await dbContext.UserCallbacks.AddRangeAsync(keyboardPresentResult.UserCallbacks, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await bot.SendTextMessageAsync(
            callbackQuery.From.Id, 
            getDirectoriesResponse.Directory?.Path ?? "Корневая папка вашего хранилища",
            replyMarkup: keyboardPresentResult.Keyboard,
            cancellationToken: cancellationToken);
    }
    
    public static OpenDirectoryCallback Create(Guid? directoryId)
    {
        return new OpenDirectoryCallback
        {
            DirectoryId = directoryId
        };
    }
}