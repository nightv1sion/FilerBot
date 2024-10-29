namespace Filer.Common.Domain.Core.Primitives;

public sealed record ValidationError : Error
{
    public ValidationError(Error[] errors)
        : base(
            "General.Validation",
            "One or more validation errors occurred",
            ErrorType.Validation)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationError FromResults(IEnumerable<Result.Result> results) =>
        new(results.Where(r => r.IsFailure).Select(r => r.Error!).ToArray());
}
