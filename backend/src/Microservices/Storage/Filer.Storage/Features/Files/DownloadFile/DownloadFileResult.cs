namespace Filer.Storage.Features.Files.DownloadFile;

public sealed record DownloadFileResult(
    string FileName,
    byte[] FileBytes);