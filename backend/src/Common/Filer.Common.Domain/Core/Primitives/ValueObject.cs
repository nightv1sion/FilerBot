using System.Diagnostics.CodeAnalysis;

namespace Filer.Common.Domain.Core.Primitives;

public abstract class ValueObject : IEqualityComparer<ValueObject>
{
    protected abstract IEnumerable<object> GetAtomicValues();

    public bool Equals(ValueObject? x, ValueObject? y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.GetAtomicValues().SequenceEqual(y.GetAtomicValues());
    }

    public int GetHashCode([DisallowNull] ValueObject obj)
    {
        HashCode hashCode = default;

        foreach (object atomicValue in obj.GetAtomicValues())
        {
            hashCode.Add(atomicValue);
        }

        return hashCode.ToHashCode();
    }
}
