namespace Filer.Storage.Shared.Entities;

public sealed class FileObject
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
    
    public required string Path { get; init; }

    public string? Extension { get; init; }

    public required long Size { get; init; }

    public required string UserId { get; init; }

    public Guid? ParentDirectoryId { get; init; }
    
    public DirectoryObject? ParentDirectory { get; init; }

    public required DateTimeOffset Created { get; init; }
    
    public DateTimeOffset? Modified { get; init; }
}