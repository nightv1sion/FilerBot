using Filer.Storage.Integration.Directories.CreateDirectory;
using Filer.Storage.Integration.Directories.GetDirectories;
using Refit;

namespace Filer.TelegramBot.Presentation.ApiClients.Storage;

public interface IStorageApi
{
    [Post("/directories")]
    Task<CreateDirectoryResponse> CreateDirectory(
        CreateDirectoryRequest request,
        CancellationToken cancellationToken);

    [Get("/directories")]
    Task<GetDirectoriesResponse> GetDirectories(
        [Query] string userId,
        [Query] Guid? parentDirectoryId,
        CancellationToken cancellationToken);
}