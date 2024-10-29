using MediatR;

namespace Filer.Common.Application.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface ICommand : IRequest;