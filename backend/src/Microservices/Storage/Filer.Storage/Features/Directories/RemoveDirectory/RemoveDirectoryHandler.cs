using Filer.Common.Application.Messaging;
using Filer.Storage.Shared.Persistence;

namespace Filer.Storage.Features.Directories.RemoveDirectory;

internal sealed class RemoveDirectoryHandler(
    ApplicationDbContext dbContext)
    : ICommandHandler<RemoveDirectoryCommand>
{
    public Task Handle(RemoveDirectoryCommand request, CancellationToken cancellationToken)
    {
        var directory = dbContext.Directories
            .Where(x => x.UserId == request.UserId)
            .FirstOrDefault(x => x.Id == request.DirectoryId);
        
        if (directory is null)
        {
            throw new InvalidOperationException($"Directory {request.DirectoryId} not found");
        }
        
        dbContext.Directories.Remove(directory);
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}