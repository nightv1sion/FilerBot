using FluentValidation;

namespace Filer.Storage.Features.Directories.RemoveDirectory;

public sealed class RemoveDirectoryValidator : AbstractValidator<RemoveDirectoryCommand>
{
    public RemoveDirectoryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        
        RuleFor(x => x.DirectoryId)
            .NotEmpty();
    }
}