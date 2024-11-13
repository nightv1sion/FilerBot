namespace Filer.Storage.Integration.Files.RemoveFile;

public sealed record RemoveFileRequest(
    string UserId,
    Guid FileId);