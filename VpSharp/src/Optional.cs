namespace VpSharp;

#pragma warning disable CA1716
#pragma warning disable CA2225

/// <summary>
///     Represents an optional value.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public readonly struct Optional<T> : IEquatable<Optional<T>>
{
    private readonly T? _value;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Optional{T}" /> struct.
    /// </summary>
    /// <param name="value"></param>
    public Optional(T? value)
    {
        HasValue = true;
        _value = value;
    }

    /// <summary>
    ///     Gets a value indicating whether this <see cref="Optional{T}" /> has a value.
    /// </summary>
    /// <value><see langword="true" /> if a value is defined; otherwise, <see langword="false" />.</value>
    public bool HasValue { get; }

    /// <summary>
    ///     Gets the underlying value of this optional.
    /// </summary>
    /// <value>The value.</value>
    public T? Value
    {
        get
        {
            if (!HasValue)
            {
                throw new InvalidOperationException("Cannot access the value of a valueless optional.");
            }

            return _value;
        }
    }

    /// <summary>
    ///     Returns a value indicating whether two instances of <see cref="Optional{T}" /> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Optional{T}" />.</param>
    /// <param name="right">The second <see cref="Optional{T}" />.</param>
    /// <value><see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</value>
    public static bool operator ==(Optional<T> left, Optional<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Returns a value indicating whether two instances of <see cref="Optional{T}" /> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Optional{T}" />.</param>
    /// <param name="right">The second <see cref="Optional{T}" />.</param>
    /// <value><see langword="true" /> if the two instances are not equal; otherwise, <see langword="false" />.</value>
    public static bool operator !=(Optional<T> left, Optional<T> right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    ///     Implicitly converts a value to a new instance of <see cref="Optional{T}" />.
    /// </summary>
    /// <returns>A new instance of <see cref="Optional{T}" />, wrapping <paramref name="value" />.</returns>
    public static implicit operator Optional<T>(T? value)
    {
        return new Optional<T>(value);
    }

    /// <summary>
    ///     Implicitly converts a value to a new instance of <see cref="Optional{T}" />.
    /// </summary>
    /// <returns>A new instance of <see cref="Optional{T}" />, wrapping <paramref name="value" />.</returns>
    public static explicit operator T?(Optional<T?> value)
    {
        return value.Value;
    }

    /// <summary>
    ///     Returns a value indicating whether this <see cref="Optional{T}" /> and another <see cref="Optional{T}" /> are equal.
    /// </summary>
    /// <param name="other">The other <see cref="Optional{T}" />.</param>
    /// <value><see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</value>
    public bool Equals(Optional<T> other)
    {
        return HasValue == other.HasValue && EqualityComparer<T?>.Default.Equals(Value, other._value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Optional<T> other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(HasValue, Value);
    }
}
