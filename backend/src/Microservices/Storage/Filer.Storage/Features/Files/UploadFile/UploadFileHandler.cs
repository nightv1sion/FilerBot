using Filer.Storage.Shared.Entities;
using Filer.Storage.Shared.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;

namespace Filer.Storage.Features.Files.UploadFile;

internal sealed class UploadFileHandler(
    IMinioClient minioClient,
    ApplicationDbContext dbContext) : IRequestHandler<UploadFileCommand, Guid>
{
    public async Task<Guid> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        string filePath;
        
        if (request.ParentDirectoryId is not null)
        {
            var directory = await dbContext.Directories
                .Where(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync(x => x.Id == request.ParentDirectoryId, cancellationToken);

            if (directory is null)
            {
                throw new InvalidOperationException($"Directory {request.ParentDirectoryId} not found");
            }
            
            filePath = $"{directory.Path}/{request.FileName}";
        }
        else
        {
            filePath = request.FileName;
        }
        
        FileObject file = new()
        {
            Id = Guid.NewGuid(),
            Name = request.FileName,
            Path = filePath,
            Extension = Path.GetExtension(request.FileName).ToLower(),
            Size = request.FileBytes.Length,
            UserId = request.UserId,
            ParentDirectoryId = request.ParentDirectoryId,
            Created = DateTimeOffset.UtcNow,
            Modified = null
        };
        
        MemoryStream stream = new(request.FileBytes);

        await minioClient.MakeBucketAsync(
            new MakeBucketArgs()
                .WithBucket(request.UserId),
            cancellationToken);

        PutObjectArgs args = new PutObjectArgs()
                .WithBucket(request.UserId)
                .WithObject(file.Id.ToString())
                .WithContentType("application/octet-stream")
                .WithStreamData(stream)
                .WithObjectSize(request.FileBytes.Length);

        await minioClient.PutObjectAsync(args, cancellationToken);
        await dbContext.Files.AddAsync(file, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return file.Id;
    }
}