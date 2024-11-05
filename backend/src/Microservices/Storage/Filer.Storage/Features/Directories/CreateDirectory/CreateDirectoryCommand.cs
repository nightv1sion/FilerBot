using MediatR;

namespace Filer.Storage.Features.Directories.CreateDirectory;

public sealed record CreateDirectoryCommand(
    string Name,
    string UserId,
    Guid? ParentDirectoryId) : IRequest<Guid>;