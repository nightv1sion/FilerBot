using Filer.Common.Presentation.Endpoints;
using Filer.Storage.Features.Directories.GetDirectories;
using Filer.Storage.Integration.Directories.GetDirectories;
using Filer.Storage.Integration.Directories.GetFile;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Filer.Storage.Features.Files.GetFile;

public sealed class GetFileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("files", Get);
    }

    private static async Task<Results<NotFound, Ok<GetFileResponse>>> Get(
        [FromQuery] string userId,
        [FromQuery] Guid fileId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new GetFileQuery(
            userId,
            fileId);
        GetFileResult? result = await sender.Send(command, cancellationToken);
        
        if (result is null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(
            new GetFileResponse(
                result.Id,
                result.Name,
                result.Path,
                result.ParentDirectoryId));
    }
}