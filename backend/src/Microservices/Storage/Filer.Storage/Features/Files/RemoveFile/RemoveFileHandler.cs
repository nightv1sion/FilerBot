using Filer.Storage.Shared.Entities;
using Filer.Storage.Shared.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;

namespace Filer.Storage.Features.Files.RemoveFile;

internal sealed class RemoveFileHandler(
    ApplicationDbContext dbContext,
    IMinioClient minioClient)
    : IRequestHandler<RemoveFileCommand>
{
    public async Task Handle(RemoveFileCommand request, CancellationToken cancellationToken)
    {
        FileObject? file = await dbContext.Files
            .Where(x => x.UserId == request.UserId)
            .FirstOrDefaultAsync(
                x => x.Id == request.FileId,
                cancellationToken);
        
        if (file is null)
        {
            throw new InvalidOperationException($"File {request.FileId} not found");
        }
        
        var args = new RemoveObjectArgs()
            .WithBucket(file.UserId)
            .WithObject(file.Id.ToString());
        
        await minioClient.RemoveObjectAsync(args, cancellationToken);
        dbContext.Files.Remove(file);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}