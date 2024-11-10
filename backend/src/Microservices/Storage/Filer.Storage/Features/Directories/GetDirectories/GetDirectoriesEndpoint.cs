using Filer.Common.Presentation.Endpoints;
using Filer.Storage.Integration.Directories.GetDirectories;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Filer.Storage.Features.Directories.GetDirectories;

public sealed class GetDirectoriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("directories", Create);
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
        GetDirectoriesResult result = await sender.Send(command, cancellationToken);
        return TypedResults.Ok(
            new GetDirectoriesResponse(
                result.ParentDirectory is not null ? 
                    new GetDirectoriesResponse.DirectoryModel(
                        result.ParentDirectory.Id,
                        result.ParentDirectory.Name,
                        result.ParentDirectory.Path,
                        result.ParentDirectory.ParentDirectoryId)
                    : null,
                result.Directories.Select(x =>
                        new GetDirectoriesResponse.DirectoryModel(
                            x.Id,
                            x.Name,
                            x.Path,
                            x.ParentDirectoryId))
                    .ToArray()));
    }
}