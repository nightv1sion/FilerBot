using MediatR;

namespace Filer.Common.Application.Messaging;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent;