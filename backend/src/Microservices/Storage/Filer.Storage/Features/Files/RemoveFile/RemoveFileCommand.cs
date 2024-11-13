using MediatR;

namespace Filer.Storage.Features.Files.RemoveFile;

public sealed record RemoveFileCommand(
    string UserId,
    Guid FileId) : IRequest;