using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Filer.TelegramBot.Presentation.Persistence;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public sealed class OpenDirectoryCallback : ICallback
{
    public Guid? DirectoryId { get; init; }
    
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
                callbackSerializer.Serialize(new OpenDirectoryCallback
                {
                    DirectoryId = dir.Id
                }));
            await dbContext.UserCallbacks.AddAsync(openDirectoryCallback, cancellationToken);
            keyboard.AddButton(dir.Name, openDirectoryCallback.Id.ToString());
        }
        
        var createDirectoryCallback = UserCallback.Create(
            Guid.NewGuid(), 
            callbackQuery.From.Id.ToString(), 
            callbackSerializer.Serialize(new CreateDirectoryCallback
            {
                ParentDirectoryId = DirectoryId
            }));
        await dbContext.UserCallbacks.AddAsync(createDirectoryCallback, cancellationToken);
        keyboard.AddButton("Создать папку", createDirectoryCallback.Id.ToString());
        
        await dbContext.SaveChangesAsync(cancellationToken);

        await bot.SendTextMessageAsync(
            callbackQuery.From.Id, 
            getDirectoriesResponse.ParentDirectory?.Path ?? "Корневая папка вашего хранилища",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
}