using Filer.Storage.Integration.Directories.RemoveDirectory;
using Filer.TelegramBot.Presentation.ApiClients.Storage;
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
        
        await storageApi.RemoveDirectory(
            new RemoveDirectoryRequest(callbackQuery.From.Id.ToString(), DirectoryId),
            cancellationToken);
        
        await bot.SendTextMessageAsync(
            callbackQuery.From.Id, 
            "Папка удалена",
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