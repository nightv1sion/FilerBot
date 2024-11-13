using Filer.Storage.Shared.Entities;
using Filer.Storage.Shared.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;

namespace Filer.Storage.Features.Files.DownloadFile;

internal sealed class DownloadFileHandler(
    ApplicationDbContext dbContext,
    IMinioClient minioClient)
    : IRequestHandler<DownloadFileQuery, DownloadFileResult?>
{
    public async Task<DownloadFileResult?> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
        FileObject? file = await dbContext.Files
            .Where(x => x.UserId == request.UserId)
            .FirstOrDefaultAsync(
                x => x.Id == request.FileId,
                cancellationToken);

        if (file is null)
        {
            return null;
        }

        var stream = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(file.UserId)
            .WithObject(file.Id.ToString())
            .WithCallbackStream(x => x.CopyTo(stream));
        
        await minioClient.GetObjectAsync(args, cancellationToken);
        
        return new DownloadFileResult(file.Name, stream.ToArray());
    }
}