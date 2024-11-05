using Filer.Common.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Filer.Storage.Features.Directories.GetDirectories;

public sealed class GetDirectoriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {

        app.MapGet("directories", Create)
            .AllowAnonymous();
    }

    private static async Task<Ok<GetDirectoriesResponse>> Create(
        [FromQuery] string userId,
        [FromQuery] Guid? parentDirectoryId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new GetDirectoriesQuery(
            userId,
            parentDirectoryId);
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Ok(new GetDirectoriesResponse(result));
    }
}