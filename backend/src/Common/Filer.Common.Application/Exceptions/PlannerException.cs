using Filer.Common.Domain.Core.Primitives;

namespace Filer.Common.Application.Exceptions;

public sealed class PlannerException : Exception
{
    public PlannerException(string requestName, Error? error = default, Exception? innerException = default)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}
