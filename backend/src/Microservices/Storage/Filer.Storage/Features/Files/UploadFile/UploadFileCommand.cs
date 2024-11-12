using MediatR;

namespace Filer.Storage.Features.Files.UploadFile;

public sealed record UploadFileCommand(
    string UserId,
    byte[] FileBytes,
    string FileName,
    Guid? ParentDirectoryId) : IRequest<Guid>;