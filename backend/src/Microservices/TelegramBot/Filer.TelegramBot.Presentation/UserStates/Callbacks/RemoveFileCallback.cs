using Filer.Storage.Integration.Files.RemoveFile;
using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Filer.TelegramBot.Presentation.Persistence;
using Filer.TelegramBot.Presentation.Telegram.Keyboard;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public sealed class RemoveFileCallback : ICallback
{
    public required Guid? ParentDirectoryId { get; init; }
    
    public required Guid FileId { get; init; }
    
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
        await storageApi.RemoveFile(
            new RemoveFileRequest(userId, FileId),
            cancellationToken);
        
        await bot.SendTextMessageAsync(
            callbackQuery.From.Id, 
            "Файл удалён",
            cancellationToken: cancellationToken);
        
        var getDirectoryResponse = await storageApi.GetDirectory(
            userId,
            ParentDirectoryId,
            cancellationToken);

        DirectoryKeyboardPresenter.Result keyboardPresentResult;
        
        if (ParentDirectoryId is null)
        {
            keyboardPresentResult = directoryKeyboardPresenter.OpenRootDirectory(
                getDirectoryResponse.SubDirectories
                    .Select(x => new DirectoryKeyboardPresenter.DirectoryButton(x.Id, x.Name))
                    .ToArray(), 
                getDirectoryResponse.Files
                    .Select(x => new DirectoryKeyboardPresenter.FileButton(x.Id, x.Name))
                    .ToArray(),
                userId);
        }
        else
        {
            keyboardPresentResult = directoryKeyboardPresenter.OpenDirectory(
                getDirectoryResponse.SubDirectories
                    .Select(x => new DirectoryKeyboardPresenter.DirectoryButton(x.Id, x.Name))
                    .ToArray(), 
                getDirectoryResponse.Files
                    .Select(x => new DirectoryKeyboardPresenter.FileButton(x.Id, x.Name))
                    .ToArray(),
                userId,
                ParentDirectoryId.Value,
                getDirectoryResponse.Directory?.ParentDirectoryId);
        }
        
        await dbContext.UserCallbacks.AddRangeAsync(keyboardPresentResult.UserCallbacks, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
                
        await bot.SendTextMessageAsync(
            userId, 
            getDirectoryResponse.Directory?.Path is not null 
                ? $"\ud83d\uddc2 {getDirectoryResponse.Directory.Path}" 
                :  "Корневая папка вашего хранилища",
            replyMarkup: keyboardPresentResult.Keyboard,
            cancellationToken: cancellationToken);
    }
    
    public static RemoveFileCallback Create(
        Guid fileId,
        Guid? parentDirectoryId)
    {
        return new RemoveFileCallback
        {
            FileId = fileId,
            ParentDirectoryId = parentDirectoryId,
        };
    }
}