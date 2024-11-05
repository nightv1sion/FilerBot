using Filer.Storage.Shared.Entities;
using Filer.Storage.Shared.Persistence;
using MediatR;

namespace Filer.Storage.Features.Directories.CreateDirectory;

internal sealed class CreateDirectoryHandler(
    ApplicationDbContext dbContext)
    : IRequestHandler<CreateDirectoryCommand, Guid>
{
    public async Task<Guid> Handle(CreateDirectoryCommand request, CancellationToken cancellationToken)
    {
        var directory = new DirectoryObject
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ParentDirectoryId = request.ParentDirectoryId,
            UserId = request.UserId,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await dbContext.Directories.AddAsync(directory, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return directory.Id;
    }
}