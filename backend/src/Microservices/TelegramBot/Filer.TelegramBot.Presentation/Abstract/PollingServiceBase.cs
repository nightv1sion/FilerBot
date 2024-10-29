namespace Filer.TelegramBot.Presentation.Abstract;

public abstract class PollingServiceBase<TReceiverService> : BackgroundService
    where TReceiverService : IReceiverService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    protected PollingServiceBase(
        IServiceProvider serviceProvider,
        ILogger<PollingServiceBase<TReceiverService>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting polling service");

        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                TReceiverService receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();

                await receiver.ReceiveAsync(stoppingToken);
            }
            catch (Exception ex)
            {
#pragma warning disable S6668
                _logger.LogError("Polling failed with exception: {Exception}", ex);
#pragma warning restore S6668

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
