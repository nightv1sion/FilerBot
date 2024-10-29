namespace Filer.Common.Domain.Core.Primitives;

public record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new(
        "General.Null",
        "Null value was provided",
        ErrorType.Failure);

    public string Message { get; private set; }

    public string Code { get; private set; }

    public ErrorType Type { get; set; }

    public Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error Failure(string code, string message)
    {
        return new Error(code, message, ErrorType.Failure);
    }

    public static Error Validation(string code, string message)
    {
        return new Error(code, message, ErrorType.Problem);
    }

    public static Error NotFound(string code, string message)
    {
        return new Error(code, message, ErrorType.NotFound);
    }

    public static Error Problem(string code, string description)
    {
        return new Error(code, description, ErrorType.Problem);
    }
}
