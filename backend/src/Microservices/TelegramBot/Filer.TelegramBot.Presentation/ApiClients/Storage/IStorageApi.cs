using Filer.Storage.Integration.Directories.CreateDirectory;
using Filer.Storage.Integration.Directories.GetDirectories;
using Filer.Storage.Integration.Directories.GetFile;
using Filer.Storage.Integration.Directories.RemoveDirectory;
using Filer.Storage.Integration.Files.UploadFIle;
using Refit;

namespace Filer.TelegramBot.Presentation.ApiClients.Storage;

public interface IStorageApi
{
    [Get("/directories")]
    Task<GetDirectoryResponse> GetDirectories(
        [Query] string userId,
        [Query] Guid? parentDirectoryId,
        CancellationToken cancellationToken);
    
    [Post("/directories")]
    Task<CreateDirectoryResponse> CreateDirectory(
        [Body] CreateDirectoryRequest request,
        CancellationToken cancellationToken);
    
    [Delete("/directories")]
    Task<RemoveDirectoryResponse> RemoveDirectory(
        [Body] RemoveDirectoryRequest request,
        CancellationToken cancellationToken);
    
    [Get("/files")]
    Task<GetFileResponse> GetFile(
        [Query] string userId,
        [Query] Guid fileId,
        CancellationToken cancellationToken);
    
    [Post("/files")]
    Task<UploadFileResponse> UploadFile(
        [Body] UploadFileRequest request,
        CancellationToken cancellationToken);
}