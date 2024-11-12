using MediatR;

namespace Filer.Storage.Features.Files.GetFile;

public sealed record GetFileQuery(
    string UserId,
    Guid FileId)
    : IRequest<GetFileResult?>;