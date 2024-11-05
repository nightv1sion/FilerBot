namespace Filer.Storage.Features.Directories.GetDirectories;

public sealed record GetDirectoriesResponse(IReadOnlyCollection<DirectoryModel> Directories);