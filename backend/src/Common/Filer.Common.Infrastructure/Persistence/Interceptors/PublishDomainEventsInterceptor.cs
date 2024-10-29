using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Filer.Common.Domain.Core.Events;
using Filer.Common.Domain.Core.Primitives;

namespace Filer.Common.Infrastructure.Persistence.Interceptors;

public sealed class PublishDomainEventsInterceptor(
    IServiceScopeFactory serviceScopeFactory)
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            PublishDomainEvents(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void PublishDomainEvents(DbContext context)
    {
        var domainEvents = context
            .ChangeTracker
            .Entries<AggregateRoot>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents.ToList();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            IServiceScope scope = serviceScopeFactory.CreateScope();
            IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            publisher.Publish(domainEvent);
        }
    }
}
