using MediatR;

namespace Filer.Storage.Features.Directories.GetDirectories;

public sealed record GetDirectoryQuery(
    string UserId,
    Guid? ParentDirectoryId)
    : IRequest<GetDirectoryResult>;