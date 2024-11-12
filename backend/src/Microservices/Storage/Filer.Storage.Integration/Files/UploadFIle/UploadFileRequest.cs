namespace Filer.Storage.Integration.Files.UploadFIle;

public sealed record UploadFileRequest(
    string UserId,
    byte[] FileBytes,
    string FileName,
    Guid? ParentDirectoryId);