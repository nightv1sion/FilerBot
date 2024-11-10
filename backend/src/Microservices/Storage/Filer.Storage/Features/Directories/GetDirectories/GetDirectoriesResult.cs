namespace Filer.Storage.Features.Directories.GetDirectories;

public sealed record GetDirectoriesResult(
    GetDirectoriesResult.DirectoryModel? ParentDirectory,
    IReadOnlyCollection<GetDirectoriesResult.DirectoryModel> Directories)
{
    public sealed record DirectoryModel(
        Guid Id,
        string Name,
        string Path,
        Guid? ParentDirectoryId);
}