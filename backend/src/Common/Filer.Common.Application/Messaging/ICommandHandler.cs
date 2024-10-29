using MediatR;

namespace Filer.Common.Application.Messaging;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : IRequest<TResponse>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : IRequest;