namespace Filer.Common.Application.Events;

public abstract class IntegrationEvent : IIntegrationEvent
{
    protected IntegrationEvent(Guid id, DateTimeOffset occurredAt)
    {
        Id = id;
        OccurredAt = occurredAt;
    }

    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        OccurredAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; init; }

    public DateTimeOffset OccurredAt { get; init; }
}
