namespace Filer.Storage.Integration.Directories.CreateDirectory;

public sealed record CreateDirectoryRequest(
    string UserId,
    string Name,
    Guid? ParentDirectoryId);