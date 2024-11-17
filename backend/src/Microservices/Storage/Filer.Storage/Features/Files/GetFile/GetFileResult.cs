namespace Filer.Storage.Features.Files.GetFile;

public sealed record GetFileResult(
    Guid Id,
    string Name,
    string Path,
    long Size,
    Guid? ParentDirectoryId);