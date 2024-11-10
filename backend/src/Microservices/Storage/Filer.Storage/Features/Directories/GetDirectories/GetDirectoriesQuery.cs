using MediatR;

namespace Filer.Storage.Features.Directories.GetDirectories;

public sealed record GetDirectoriesQuery(
    string UserId,
    Guid? ParentDirectoryId)
    : IRequest<GetDirectoriesResult>;