namespace Filer.Storage.Features.Directories.GetDirectories;

public sealed record GetDirectoryResult(
    GetDirectoryResult.DirectoryModel? ParentDirectory,
    IReadOnlyCollection<GetDirectoryResult.FileModel> Files,
    IReadOnlyCollection<GetDirectoryResult.DirectoryModel> Directories)
{
    public sealed record DirectoryModel(
        Guid Id,
        string Name,
        string Path,
        Guid? ParentDirectoryId);
    
    public sealed record FileModel(
        Guid Id,
        string Name);
}