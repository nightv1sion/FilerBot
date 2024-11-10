using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Filer.TelegramBot.Presentation.Persistence;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public sealed class OpenDirectoryCallback : ICallback
{
    public required Guid DirectoryId { get; init; }
    
    public async Task Handle(
        IServiceProvider serviceProvider,
        CallbackQuery callbackQuery,
        CancellationToken cancellationToken)
    {
        var storageApi = serviceProvider.GetRequiredService<IStorageApi>();
        var callbackSerializer = serviceProvider.GetRequiredService<CallbackSerializer>();
        var bot = serviceProvider.GetRequiredService<ITelegramBotClient>();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        
        var getDirectoriesResponse = await storageApi.GetDirectories(
            callbackQuery.From.Id.ToString(),
            DirectoryId,
            cancellationToken);

        var keyboard = new InlineKeyboardMarkup();

        foreach (var dir in getDirectoriesResponse.Directories)
        {
            var openDirectoryCallback = UserCallback.Create(
                Guid.NewGuid(), 
                callbackQuery.From.Id.ToString(), 
                callbackSerializer.Serialize(Create(dir.Id)));
            
            await dbContext.UserCallbacks.AddAsync(openDirectoryCallback, cancellationToken);
            keyboard.AddNewRow();
            keyboard.AddButton(dir.Name, openDirectoryCallback.Id.ToString());
        }
        
        keyboard.AddNewRow();
        
        var createDirectoryCallback = UserCallback.Create(
            Guid.NewGuid(), 
            callbackQuery.From.Id.ToString(), 
            callbackSerializer.Serialize(CreateDirectoryCallback.Create(DirectoryId)));
        await dbContext.UserCallbacks.AddAsync(createDirectoryCallback, cancellationToken);
        keyboard.AddButton("Создать папку", createDirectoryCallback.Id.ToString());

        var removeDirectoryCallback = UserCallback.Create(
            Guid.NewGuid(), 
            callbackQuery.From.Id.ToString(), 
            callbackSerializer.Serialize(RemoveDirectoryCallback.Create(DirectoryId)));
        await dbContext.UserCallbacks.AddAsync(removeDirectoryCallback, cancellationToken);
        keyboard.AddButton("Удалить папку", removeDirectoryCallback.Id.ToString());
        
        await dbContext.SaveChangesAsync(cancellationToken);

        await bot.SendTextMessageAsync(
            callbackQuery.From.Id, 
            getDirectoriesResponse.ParentDirectory?.Path ?? "Корневая папка вашего хранилища",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
    
    public static OpenDirectoryCallback Create(Guid directoryId)
    {
        return new OpenDirectoryCallback
        {
            DirectoryId = directoryId
        };
    }
}