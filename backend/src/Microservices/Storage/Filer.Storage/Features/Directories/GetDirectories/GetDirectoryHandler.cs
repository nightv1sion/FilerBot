using Filer.Storage.Shared.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Filer.Storage.Features.Directories.GetDirectories;

internal sealed class GetDirectoryHandler(ApplicationDbContext dbContext)
    : IRequestHandler<GetDirectoryQuery, GetDirectoryResult>
{
    public async Task<GetDirectoryResult> Handle(GetDirectoryQuery request, CancellationToken cancellationToken)
    {
        GetDirectoryResult.DirectoryModel? parentDirectory = null;
        
        if (request.ParentDirectoryId is not null)
        {
            parentDirectory = await dbContext
                .Directories
                .Where(x => x.UserId == request.UserId)
                .Where(x => x.Id == request.ParentDirectoryId)
                .Select(x => new GetDirectoryResult.DirectoryModel(
                    x.Id,
                    x.Name,
                    x.Path,
                    x.ParentDirectoryId))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
        
        var directories = await dbContext
            .Directories
            .Where(x => x.UserId == request.UserId)
            .Where(x => x.ParentDirectoryId == request.ParentDirectoryId)
            .Select(x => new GetDirectoryResult.DirectoryModel(
                x.Id,
                x.Name,
                x.Path,
                x.ParentDirectoryId))
            .ToArrayAsync(cancellationToken: cancellationToken);
        
        var files = await dbContext
            .Files
            .Where(x => x.UserId == request.UserId)
            .Where(x => x.ParentDirectoryId == request.ParentDirectoryId)
            .Select(x => new GetDirectoryResult.FileModel(
                x.Id,
                x.Name))
            .ToArrayAsync(cancellationToken: cancellationToken);
        
        return new GetDirectoryResult(
            parentDirectory,
            files,
            directories);
    }
}