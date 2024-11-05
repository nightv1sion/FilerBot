namespace Filer.Storage.Shared.Entities;

public sealed class FileObject
{
    public required Guid Id { get; init; }

    public required string FileName { get; init; }

    public required string Extension { get; init; }

    public required string Size { get; init; }

    public required string UserId { get; init; }

    public required DateTimeOffset Created { get; init; }
}