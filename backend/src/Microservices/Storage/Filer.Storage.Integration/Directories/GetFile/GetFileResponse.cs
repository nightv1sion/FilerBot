namespace Filer.Storage.Integration.Directories.GetFile;

public sealed record GetFileResponse(
    Guid Id,
    string Name,
    string Path,
    Guid? ParentDirectoryId);