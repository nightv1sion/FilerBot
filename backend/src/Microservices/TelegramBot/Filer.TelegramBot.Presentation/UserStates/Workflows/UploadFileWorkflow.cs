using Filer.Storage.Integration.Directories.CreateDirectory;
using Filer.Storage.Integration.Files.UploadFIle;
using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Filer.TelegramBot.Presentation.Persistence;
using Filer.TelegramBot.Presentation.Telegram.Keyboard;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Filer.TelegramBot.Presentation.UserStates.Workflows;

public sealed class UploadFileWorkflow : IWorkflow
{
    public Guid? DirectoryId { get; init; }
    
    public bool IsCompleted { get; set; }
    
    public UploadFileWorkflowStep WorkflowStep { get; set; }
    
    public async Task Start(IServiceProvider serviceProvider, Message message, CancellationToken cancellationToken)
    {
        var botClient = serviceProvider.GetRequiredService<ITelegramBotClient>();

        await botClient.SendTextMessageAsync(
            message.Chat.Id,
            "Загрузите файл",
            cancellationToken: cancellationToken);

        WorkflowStep = UploadFileWorkflowStep.FileAsked;
    }

    public async Task Continue(IServiceProvider serviceProvider, Message message, CancellationToken cancellationToken)
    {
        var storageApi = serviceProvider.GetRequiredService<IStorageApi>();
        var bot = serviceProvider.GetRequiredService<ITelegramBotClient>();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var directoryKeyboardPresenter = serviceProvider.GetRequiredService<DirectoryKeyboardPresenter>();
        
        var userId = message.Chat.Id.ToString();
        
        switch (WorkflowStep)
        {
            case UploadFileWorkflowStep.FileAsked:
                var document = message.Document;
                if (document is null)
                {
                    await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "Ошибка при загрузке файла. Попробуйте ещё раз",
                        cancellationToken: cancellationToken);
                    return;
                }

                var stream = new MemoryStream();
                var file = await bot.GetInfoAndDownloadFileAsync(
                    document.FileId,
                    stream,
                    cancellationToken: cancellationToken);
                stream.Position = 0;
                
                UploadFileResponse uploadFileResponse = await storageApi.UploadFile(
                    new UploadFileRequest(
                        userId,
                        stream.ToArray(),
                        document.FileName!,
                        DirectoryId),
                    cancellationToken);
                IsCompleted = true;
                        
                await bot.SendTextMessageAsync(
                    message.Chat.Id,
                    "Файл загружен",
                    cancellationToken: cancellationToken);
                
                var getDirectoryResponse = await storageApi.GetDirectory(
                    userId,
                    DirectoryId,
                    cancellationToken);
                
                DirectoryKeyboardPresenter.Result keyboardPresentResult;
                
                if (DirectoryId is null)
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
                        DirectoryId.Value,
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

                break;

            case UploadFileWorkflowStep.None:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static UploadFileWorkflow Create(Guid? parentDirectoryId)
    {
        return new UploadFileWorkflow
        {
            DirectoryId = parentDirectoryId,
            WorkflowStep = UploadFileWorkflowStep.None,
        };
    }
}