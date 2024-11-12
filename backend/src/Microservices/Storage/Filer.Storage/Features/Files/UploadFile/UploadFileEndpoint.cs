using System.Net.Mime;
using Filer.Common.Presentation.Endpoints;
using Filer.Storage.Integration.Files.UploadFIle;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Filer.Storage.Features.Files.UploadFile;

public sealed class UploadFileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("files", UploadFile)
            .Accepts<UploadFileRequest>(MediaTypeNames.Application.Json);
    }
    
    private static async Task<Ok<UploadFileResponse>> UploadFile(
        [FromBody] UploadFileRequest request,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UploadFileCommand(
            request.UserId,
            request.FileBytes,
            request.FileName,
            request.ParentDirectoryId);
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Ok(new UploadFileResponse(result));
    }
}