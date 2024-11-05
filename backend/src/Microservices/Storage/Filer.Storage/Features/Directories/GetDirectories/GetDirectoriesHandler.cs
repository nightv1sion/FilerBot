using Filer.Storage.Shared.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Filer.Storage.Features.Directories.GetDirectories;

internal sealed class GetDirectoriesHandler(ApplicationDbContext dbContext)
    : IRequestHandler<GetDirectoriesQuery, IReadOnlyCollection<DirectoryModel>>
{
    public async Task<IReadOnlyCollection<DirectoryModel>> Handle(GetDirectoriesQuery request, CancellationToken cancellationToken)
    {
        var directories = await dbContext
            .Directories
            .Where(x => x.UserId == request.UserId)
            .Where(x => x.ParentDirectoryId == request.ParentDirectoryId)
            .Select(x => new DirectoryModel(x.Id, x.Name, x.ParentDirectoryId))
            .ToArrayAsync(cancellationToken: cancellationToken);
        
        return directories;
    }
}