namespace Filer.Common.Domain.Core.Utility;

public static class Ensure
{
    public static void NotEmpty(string value, string message, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty(Guid value, string message, string argumentName)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty(DateTime value, string message, string argumentName)
    {
        if (value == default)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty<T>(T value, string message, string argumentName) where T : class
    {
        if (value == null)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty<T>(IEnumerable<T> value, string message, string argumentName)
    {
        if (value == null || !value.Any())
        {
            throw new ArgumentException(message, argumentName);
        }
    }
}