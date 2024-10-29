namespace Filer.TelegramBot.Presentation.Abstract;

public interface IMessageSender
{
    Task SendMessage(long destinationUserId, string message, CancellationToken cancellationToken);
}
