namespace Filer.Storage.Shared.Entities;

public sealed class DirectoryObject
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }

    public required string Path { get; init; }
    
    public Guid? ParentDirectoryId { get; init; }
    
    public DirectoryObject? ParentDirectory { get; init; }
    
    public List<DirectoryObject> SubDirectories { get; init; } = new();
    
    public List<FileObject> Files { get; init; } = new();

    public required DateTimeOffset CreatedAt { get; init; }

    public required string UserId { get; init; }
    
    public DateTimeOffset? ModifiedAt { get; init; }
}