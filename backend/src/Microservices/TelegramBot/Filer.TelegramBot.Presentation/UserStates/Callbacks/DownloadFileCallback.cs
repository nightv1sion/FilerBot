using Telegram.Bot.Types;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public sealed class DownloadFileCallback : ICallback
{
    public required Guid FileId { get; init; }
    
    public Task Handle(IServiceProvider serviceProvider, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    public static DownloadFileCallback Create(Guid fileId)
    {
        return new DownloadFileCallback
        {
            FileId = fileId
        };
    }
}