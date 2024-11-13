using MediatR;

namespace Filer.Storage.Features.Files.DownloadFile;

public sealed record DownloadFileQuery(
    string UserId,
    Guid FileId) : IRequest<DownloadFileResult?>;