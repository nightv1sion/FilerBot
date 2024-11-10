using Filer.Storage.Shared.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Filer.Storage.Features.Directories.GetDirectories;

internal sealed class GetDirectoriesHandler(ApplicationDbContext dbContext)
    : IRequestHandler<GetDirectoriesQuery, GetDirectoriesResult>
{
    public async Task<GetDirectoriesResult> Handle(GetDirectoriesQuery request, CancellationToken cancellationToken)
    {
        GetDirectoriesResult.DirectoryModel? parentDirectory = null;
        
        if (request.ParentDirectoryId is not null)
        {
            parentDirectory = await dbContext
                .Directories
                .Where(x => x.UserId == request.UserId)
                .Where(x => x.Id == request.ParentDirectoryId)
                .Select(x => new GetDirectoriesResult.DirectoryModel(
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
            .Select(x => new GetDirectoriesResult.DirectoryModel(
                x.Id,
                x.Name,
                x.Path,
                x.ParentDirectoryId))
            .ToArrayAsync(cancellationToken: cancellationToken);
        
        return new GetDirectoriesResult(
            parentDirectory,
            directories);
    }
}