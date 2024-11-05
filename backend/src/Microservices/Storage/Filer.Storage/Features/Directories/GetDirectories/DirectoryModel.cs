namespace Filer.Storage.Features.Directories.GetDirectories;

public sealed record DirectoryModel(
    Guid Id,
    string Name,
    Guid? ParentDirectoryId);