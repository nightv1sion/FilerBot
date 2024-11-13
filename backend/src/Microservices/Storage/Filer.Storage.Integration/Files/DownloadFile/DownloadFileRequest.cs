namespace Filer.Storage.Integration.Files.DownloadFile;

public sealed record DownloadFileRequest(
    string UserId,
    Guid FileId);