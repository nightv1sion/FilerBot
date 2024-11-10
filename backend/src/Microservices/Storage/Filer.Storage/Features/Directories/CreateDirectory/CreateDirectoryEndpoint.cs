using System.Net.Mime;
using Filer.Common.Presentation.Endpoints;
using Filer.Storage.Integration.Directories.CreateDirectory;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Filer.Storage.Features.Directories.CreateDirectory;

public sealed class CreateDirectoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("directories", Create)
            .Accepts<CreateDirectoryRequest>(MediaTypeNames.Application.Json)
            .AllowAnonymous();
    }

    private static async Task<Ok<CreateDirectoryResponse>> Create(
        [FromBody] CreateDirectoryRequest request,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateDirectoryCommand(
            request.UserId,
            request.Name,
            request.ParentDirectoryId);
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Ok(new CreateDirectoryResponse(result));
    }
}