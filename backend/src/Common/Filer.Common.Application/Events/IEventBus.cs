namespace Filer.Common.Application.Events;

public interface IEventBus
{
    Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent;
    
    Task PublishBatchAsync<T>(IEnumerable<T> integrationEvents, CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent;
}
