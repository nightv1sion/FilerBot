namespace Filer.Common.Domain.Core.Primitives.Maybe;

public sealed class Maybe<TValue> : IEquatable<Maybe<TValue>>
{
    private readonly TValue _value;

    private Maybe(TValue value)
    {
        _value = value;
        if (_value is null)
        {
            HasValue = false;
        }
    }

    public TValue Value => HasValue
        ? _value
        : throw new InvalidOperationException("The value can not be accessed because it does not exist.");

    public bool HasValue { get; }

    public bool HasNoValue => !HasValue;

    public static Maybe<TValue> From(TValue value) => new(value);

    public bool Equals(Maybe<TValue>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (HasNoValue && other.HasNoValue)
        {
            return true;
        }

        if (HasNoValue || other.HasNoValue)
        {
            return false;
        }

        return Value!.Equals(other.Value);
    }

    public override bool Equals(object? obj) =>
        obj switch
        {
            null => false,
            TValue value => Equals(new Maybe<TValue>(value)),
            _ => false
        };

    public override int GetHashCode()
    {
        return HasValue ? Value!.GetHashCode() : 0;
    }
}