using System.Net.Mime;
using Filer.Common.Presentation.Endpoints;
using Filer.Storage.Features.Files.UploadFile;
using Filer.Storage.Integration.Files.DownloadFile;
using Filer.Storage.Integration.Files.UploadFIle;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Filer.Storage.Features.Files.DownloadFile;

public sealed class DownloadFileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("files/download", DownloadFile)
            .Accepts<DownloadFileRequest>(MediaTypeNames.Application.Json);
    }
    
    private static async Task<Results<Ok<DownloadFileResponse>, NotFound>> DownloadFile(
        [FromBody] DownloadFileRequest request,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new DownloadFileQuery(
            request.UserId,
            request.FileId);
        DownloadFileResult? result = await sender.Send(command, cancellationToken);

        if (result is null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(
            new DownloadFileResponse(
                result.FileName,
                result.FileBytes));
    }
}