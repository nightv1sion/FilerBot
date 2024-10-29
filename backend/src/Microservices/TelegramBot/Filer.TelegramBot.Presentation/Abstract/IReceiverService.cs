namespace Filer.TelegramBot.Presentation.Abstract;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}
