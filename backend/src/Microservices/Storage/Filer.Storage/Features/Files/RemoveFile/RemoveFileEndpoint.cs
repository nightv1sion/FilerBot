using System.Net.Mime;
using Filer.Common.Presentation.Endpoints;
using Filer.Storage.Integration.Files.RemoveFile;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Filer.Storage.Features.Files.RemoveFile;

public class RemoveFileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("files", UploadFile)
            .Accepts<RemoveFileRequest>(MediaTypeNames.Application.Json);
    }
    
    private static async Task<NoContent> UploadFile(
        [FromBody] RemoveFileRequest request,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new RemoveFileCommand(
            request.UserId,
            request.FileId);
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }
}