namespace Filer.Storage.Integration.Directories.GetDirectories;

public sealed record GetDirectoriesResponse(
    GetDirectoriesResponse.DirectoryModel? Directory,
    IReadOnlyCollection<GetDirectoriesResponse.DirectoryModel> SubDirectories)
{
    public sealed record DirectoryModel(
        Guid Id,
        string Name,
        string Path,
        Guid? ParentDirectoryId);
};