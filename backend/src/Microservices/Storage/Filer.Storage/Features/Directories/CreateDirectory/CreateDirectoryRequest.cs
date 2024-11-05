namespace Filer.Storage.Features.Directories.CreateDirectory;

public sealed record CreateDirectoryRequest(
    string UserId,
    string Name,
    Guid? ParentDirectoryId);