using MediatR;

namespace Filer.Storage.Features.FileObjects;

public sealed record UploadFileObjectCommand : IRequest<Guid>;