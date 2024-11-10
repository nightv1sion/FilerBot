using Filer.Storage.Shared.Entities;
using Filer.Storage.Shared.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Filer.Storage.Features.Directories.CreateDirectory;

internal sealed class CreateDirectoryHandler(
    ApplicationDbContext dbContext)
    : IRequestHandler<CreateDirectoryCommand, Guid>
{
    public async Task<Guid> Handle(CreateDirectoryCommand request, CancellationToken cancellationToken)
    {
        string path = request.Name;
        
        if (request.ParentDirectoryId is not null)
        {
            var parentDirectory = await dbContext
                .Directories
                .Where(x => x.UserId == request.UserId)
                .Where(x => x.Id == request.ParentDirectoryId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            if (parentDirectory is null)
            {
                throw new InvalidOperationException($"Parent directory with id {request.ParentDirectoryId} not found");
            }
            
            path = $"{parentDirectory.Path}/{path}";
        }
        
        var directory = new DirectoryObject
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ParentDirectoryId = request.ParentDirectoryId,
            UserId = request.UserId,
            Path = path,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await dbContext.Directories.AddAsync(directory, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return directory.Id;
    }
}