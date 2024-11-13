namespace Filer.Storage.Integration.Files.DownloadFile;

public sealed record DownloadFileResponse(
    string FileName,
    byte[] FileBytes);