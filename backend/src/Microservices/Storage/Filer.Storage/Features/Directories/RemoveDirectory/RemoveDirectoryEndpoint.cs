using System.Net.Mime;
using Filer.Common.Presentation.Endpoints;
using Filer.Storage.Integration.Directories.RemoveDirectory;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Filer.Storage.Features.Directories.RemoveDirectory;

public sealed class RemoveDirectoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("directories", Remove)
            .Accepts<RemoveDirectoryRequest>(MediaTypeNames.Application.Json);
    }

    private static async Task<NoContent> Remove(
        [FromBody] RemoveDirectoryRequest request,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new RemoveDirectoryCommand(request.UserId, request.DirectoryId);
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }
}