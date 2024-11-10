namespace Filer.Storage.Integration.Directories.GetDirectories;

public sealed record GetDirectoriesResponse(
    GetDirectoriesResponse.DirectoryModel? ParentDirectory,
    IReadOnlyCollection<GetDirectoriesResponse.DirectoryModel> Directories)
{
    public sealed record DirectoryModel(
        Guid Id,
        string Name,
        string Path,
        Guid? ParentDirectoryId);
};