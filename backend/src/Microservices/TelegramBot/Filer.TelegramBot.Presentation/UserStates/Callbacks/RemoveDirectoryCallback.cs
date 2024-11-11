using Filer.Storage.Integration.Directories.RemoveDirectory;
using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Filer.TelegramBot.Presentation.Persistence;
using Filer.TelegramBot.Presentation.Telegram.Keyboard;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public sealed class RemoveDirectoryCallback : ICallback
{
    public required Guid DirectoryId { get; init; }

    public async Task Handle(
        IServiceProvider serviceProvider,
        CallbackQuery callbackQuery,
        CancellationToken cancellationToken)
    {
        var storageApi = serviceProvider.GetRequiredService<IStorageApi>();
        var bot = serviceProvider.GetRequiredService<ITelegramBotClient>();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var directoryKeyboardPresenter = serviceProvider.GetRequiredService<DirectoryKeyboardPresenter>();
        
        var userId = callbackQuery.From.Id.ToString();
        
        var removeDirectoryResponse = await storageApi.RemoveDirectory(
            new RemoveDirectoryRequest(userId, DirectoryId),
            cancellationToken);
        
        await bot.SendTextMessageAsync(
            callbackQuery.From.Id, 
            "Папка удалена",
            cancellationToken: cancellationToken);
        
        var getDirectoriesResponse = await storageApi.GetDirectories(
            userId,
            removeDirectoryResponse.ParentRemovedDirectoryId,
            cancellationToken);

        DirectoryKeyboardPresenter.Result keyboardPresentResult;
        
        if (removeDirectoryResponse.ParentRemovedDirectoryId is null)
        {
            keyboardPresentResult = directoryKeyboardPresenter.OpenRootDirectory(
                getDirectoriesResponse.SubDirectories
                    .Select(x => new DirectoryKeyboardPresenter.DirectoryButton(x.Id, x.Name))
                    .ToArray(), 
                userId);
        }
        else
        {
            keyboardPresentResult = directoryKeyboardPresenter.OpenDirectory(
                getDirectoriesResponse.SubDirectories
                    .Select(x => new DirectoryKeyboardPresenter.DirectoryButton(x.Id, x.Name))
                    .ToArray(), 
                userId,
                removeDirectoryResponse.ParentRemovedDirectoryId.Value,
                getDirectoriesResponse.Directory?.ParentDirectoryId);
        }
        
        await dbContext.UserCallbacks.AddRangeAsync(keyboardPresentResult.UserCallbacks, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
                
        await bot.SendTextMessageAsync(
            userId, 
            getDirectoriesResponse.Directory?.Path ?? "Корневая папка вашего хранилища",
            replyMarkup: keyboardPresentResult.Keyboard,
            cancellationToken: cancellationToken);
    }
    
    public static RemoveDirectoryCallback Create(Guid directoryId)
    {
        return new RemoveDirectoryCallback
        {
            DirectoryId = directoryId
        };
    }
}