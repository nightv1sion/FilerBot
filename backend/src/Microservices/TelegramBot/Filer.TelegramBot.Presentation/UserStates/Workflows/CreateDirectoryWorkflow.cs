using Filer.Storage.Integration.Directories.CreateDirectory;
using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Filer.TelegramBot.Presentation.Persistence;
using Filer.TelegramBot.Presentation.Telegram.Keyboard;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Filer.TelegramBot.Presentation.UserStates.Workflows;

public sealed class CreateDirectoryWorkflow : IWorkflow
{
    public Guid? ParentDirectoryId { get; init; }
    
    public string? DirectoryName { get; set; }
    
    public bool IsCompleted { get; private set; }

    public CreateDirectoryWorkflowStep WorkflowStep { get; set; }
    
    public async Task Start(
        IServiceProvider serviceProvider,
        Message message,
        CancellationToken cancellationToken)
    {
        var botClient = serviceProvider.GetRequiredService<ITelegramBotClient>();

        await botClient.SendTextMessageAsync(
            message.Chat.Id,
            "Напишите название директории",
            cancellationToken: cancellationToken);

        WorkflowStep = CreateDirectoryWorkflowStep.CreateDirectoryNameAsked;
    }

    public async Task Continue(
        IServiceProvider serviceProvider,
        Message message,
        CancellationToken cancellationToken)
    {
        var storageApi = serviceProvider.GetRequiredService<IStorageApi>();
        var bot = serviceProvider.GetRequiredService<ITelegramBotClient>();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var directoryKeyboardPresenter = serviceProvider.GetRequiredService<DirectoryKeyboardPresenter>();
        
        var userId = message.Chat.Id.ToString();
        
        switch (WorkflowStep)
        {
            case CreateDirectoryWorkflowStep.CreateDirectoryNameAsked:
                CreateDirectoryResponse createDirectoryResponse = await storageApi.CreateDirectory(
                    new CreateDirectoryRequest(
                        userId,
                        message.Text!,
                        ParentDirectoryId),
                    cancellationToken);
                IsCompleted = true;
                        
                await bot.SendTextMessageAsync(
                    message.Chat.Id,
                    "Папка создана",
                    cancellationToken: cancellationToken);
                
                var getDirectoryResponse = await storageApi.GetDirectory(
                    userId,
                    createDirectoryResponse.DirectoryId,
                    cancellationToken);

                DirectoryKeyboardPresenter.Result keyboardPresentResult = directoryKeyboardPresenter.OpenDirectory(
                    getDirectoryResponse.SubDirectories
                        .Select(x => new DirectoryKeyboardPresenter.DirectoryButton(x.Id, x.Name))
                        .ToArray(), 
                    getDirectoryResponse.Files
                        .Select(x => new DirectoryKeyboardPresenter.FileButton(x.Id, x.Name))
                        .ToArray(),
                    userId,
                    createDirectoryResponse.DirectoryId,
                    getDirectoryResponse.Directory?.ParentDirectoryId);
        
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

            case CreateDirectoryWorkflowStep.None:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static CreateDirectoryWorkflow Create(
        Guid? parentDirectoryId)
    {
        return new CreateDirectoryWorkflow
        {
            ParentDirectoryId = parentDirectoryId,
            WorkflowStep = CreateDirectoryWorkflowStep.None,
            DirectoryName = null,
            IsCompleted = false,
        };
    }
}