using Filer.Common.Presentation.Endpoints;
using Filer.Storage.Integration.Directories.GetDirectories;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Filer.Storage.Features.Directories.GetDirectories;

public sealed class GetDirectoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("directories", Create);
    }

    private static async Task<Ok<GetDirectoryResponse>> Create(
        [FromQuery] string userId,
        [FromQuery] Guid? parentDirectoryId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new GetDirectoryQuery(
            userId,
            parentDirectoryId);
        GetDirectoryResult result = await sender.Send(command, cancellationToken);
        return TypedResults.Ok(
            new GetDirectoryResponse(
                result.ParentDirectory is not null ? 
                    new GetDirectoryResponse.DirectoryModel(
                        result.ParentDirectory.Id,
                        result.ParentDirectory.Name,
                        result.ParentDirectory.Path,
                        result.ParentDirectory.ParentDirectoryId)
                    : null,
                result.Files.Select(x =>
                    new GetDirectoryResponse.FileModel(x.Id, x.Name))
                    .ToArray(),
                result.Directories.Select(x =>
                        new GetDirectoryResponse.DirectoryModel(
                            x.Id,
                            x.Name,
                            x.Path,
                            x.ParentDirectoryId))
                    .ToArray()));
    }
}