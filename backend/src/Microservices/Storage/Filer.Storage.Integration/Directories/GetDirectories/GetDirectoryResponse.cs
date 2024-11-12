namespace Filer.Storage.Integration.Directories.GetDirectories;

public sealed record GetDirectoryResponse(
    GetDirectoryResponse.DirectoryModel? Directory,
    IReadOnlyCollection<GetDirectoryResponse.FileModel> Files,
    IReadOnlyCollection<GetDirectoryResponse.DirectoryModel> SubDirectories)
{
    public sealed record DirectoryModel(
        Guid Id,
        string Name,
        string Path,
        Guid? ParentDirectoryId);

    public sealed record FileModel(
        Guid Id,
        string Name);
};