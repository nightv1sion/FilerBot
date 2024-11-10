using Filer.Storage.Integration.Directories.CreateDirectory;
using Filer.TelegramBot.Presentation.ApiClients.Storage;
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
        
        switch (WorkflowStep)
        {
            case CreateDirectoryWorkflowStep.CreateDirectoryNameAsked:
                
                await storageApi.CreateDirectory(
                    new CreateDirectoryRequest(
                        message.Chat.Id.ToString(),
                        message.Text!,
                        ParentDirectoryId),
                    cancellationToken);
                IsCompleted = true;
                break;
            
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