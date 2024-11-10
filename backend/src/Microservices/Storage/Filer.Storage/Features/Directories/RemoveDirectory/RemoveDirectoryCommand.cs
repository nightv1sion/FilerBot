using MediatR;

namespace Filer.Storage.Features.Directories.RemoveDirectory;

public sealed record RemoveDirectoryCommand(
    string UserId,
    Guid DirectoryId) : IRequest;