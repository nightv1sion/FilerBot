using Filer.Storage.Shared.Entities;
using Filer.Storage.Shared.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Filer.Storage.Features.Files.GetFile;

internal sealed class GetFileHandler(
    ApplicationDbContext dbContext)
    : IRequestHandler<GetFileQuery, GetFileResult?>
{
    public async Task<GetFileResult?> Handle(GetFileQuery request, CancellationToken cancellationToken)
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
        
        return new GetFileResult(
            file.Id,
            file.Name,
            file.Path,
            file.Size,
            file.ParentDirectoryId);
    }
}