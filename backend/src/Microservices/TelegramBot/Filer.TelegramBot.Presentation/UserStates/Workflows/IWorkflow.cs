using Telegram.Bot.Types;

namespace Filer.TelegramBot.Presentation.UserStates.Workflows;

public interface IWorkflow
{
    bool IsCompleted { get; }
    
    Task Start(
        IServiceProvider serviceProvider,
        Message message,
        CancellationToken cancellationToken);
    
    Task Continue(
        IServiceProvider serviceProvider,
        Message message,
        CancellationToken cancellationToken);
}