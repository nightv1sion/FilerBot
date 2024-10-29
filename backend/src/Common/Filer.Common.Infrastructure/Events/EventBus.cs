using MassTransit;
using Filer.Common.Application.Events;

namespace Filer.Common.Infrastructure.Events;

internal sealed class EventBus(IBus bus) : IEventBus
{
    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent
    {
        await bus.Publish(integrationEvent, cancellationToken);
    }

    public async Task PublishBatchAsync<T>(IEnumerable<T> integrationEvents, CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent
    {
        await bus.PublishBatch(integrationEvents, cancellationToken);
    }
}
