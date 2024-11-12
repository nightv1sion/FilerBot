using Filer.TelegramBot.Presentation.Persistence;
using Filer.TelegramBot.Presentation.UserStates.Workflows;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public sealed class UploadFileCallback : ICallback
{
    public Guid? ParentDirectoryId { get; init; }

    public async Task Handle(IServiceProvider serviceProvider, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var workflowSerializer = serviceProvider.GetRequiredService<WorkflowSerializer>();
        
        UserState? userState = await dbContext.UserStates
            .FirstOrDefaultAsync(
                x => x.UserId == callbackQuery.From.Id.ToString(),
                cancellationToken);
        
        if (userState is null)
        {
            throw new InvalidOperationException($"User {callbackQuery.From.Id} state not found");
        }

        var workflow = UploadFileWorkflow.Create(ParentDirectoryId);

        await workflow.Start(
            serviceProvider,
            callbackQuery.Message!,
            cancellationToken);

        var userWorkflow = UserWorkflow.Create(
            Guid.NewGuid(),
            userState.UserId,
            workflowSerializer.Serialize(workflow));
        userState.CurrentWorkflow = userWorkflow;
        
        await dbContext.UserWorkflows.AddAsync(userWorkflow, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public static UploadFileCallback Create(Guid? parentDirectoryId)
    {
        return new UploadFileCallback
        {
            ParentDirectoryId = parentDirectoryId
        };
    }
}