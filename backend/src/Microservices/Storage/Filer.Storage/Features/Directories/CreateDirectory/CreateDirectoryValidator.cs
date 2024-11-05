using FluentValidation;

namespace Filer.Storage.Features.Directories.CreateDirectory;

public sealed class CreateDirectoryValidator : AbstractValidator<CreateDirectoryCommand>
{
    public CreateDirectoryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .NotNull();
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull();
    }
}