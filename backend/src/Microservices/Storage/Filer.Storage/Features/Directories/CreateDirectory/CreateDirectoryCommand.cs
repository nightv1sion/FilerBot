using MediatR;

namespace Filer.Storage.Features.Directories.CreateDirectory;

public sealed record CreateDirectoryCommand(
    string UserId,
    string Name,
    Guid? ParentDirectoryId) : IRequest<Guid>;