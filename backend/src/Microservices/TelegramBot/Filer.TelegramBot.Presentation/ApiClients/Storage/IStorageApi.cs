using Filer.Storage.Integration.Directories.CreateDirectory;
using Filer.Storage.Integration.Directories.GetDirectories;
using Filer.Storage.Integration.Directories.RemoveDirectory;
using Refit;

namespace Filer.TelegramBot.Presentation.ApiClients.Storage;

public interface IStorageApi
{
    [Get("/directories")]
    Task<GetDirectoriesResponse> GetDirectories(
        [Query] string userId,
        [Query] Guid? parentDirectoryId,
        CancellationToken cancellationToken);
    
    [Post("/directories")]
    Task<CreateDirectoryResponse> CreateDirectory(
        [Body] CreateDirectoryRequest request,
        CancellationToken cancellationToken);
    
    [Delete("/directories")]
    Task RemoveDirectory(
        [Body] RemoveDirectoryRequest request,
        CancellationToken cancellationToken);
}