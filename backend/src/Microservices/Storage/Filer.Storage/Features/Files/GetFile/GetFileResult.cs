namespace Filer.Storage.Features.Files.GetFile;

public sealed record GetFileResult(
    Guid Id,
    string Name,
    string Path,
    Guid? ParentDirectoryId);