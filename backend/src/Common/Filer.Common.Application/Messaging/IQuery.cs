using MediatR;

namespace Filer.Common.Application.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>;