namespace Filer.Storage.Integration.Directories.RemoveDirectory;

public sealed record RemoveDirectoryRequest(string UserId, Guid DirectoryId);