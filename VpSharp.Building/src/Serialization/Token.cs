namespace VpSharp.Building.Serialization;

/// <summary>
///     Represents a token.
/// </summary>
public ref struct Token
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Token" /> class.
    /// </summary>
    /// <param name="type">The token type.</param>
    /// <param name="value">The token value.</param>
    public Token(TokenType type, ReadOnlySpan<char> value)
    {
        Type = type;
        ValueSpan = value;
        Value = value.ToString();
    }

    /// <summary>
    ///     Gets the token type.
    /// </summary>
    /// <value>The token type.</value>
    public TokenType Type { get; }

    /// <summary>
    ///     Gets the token value as a span of characters.
    /// </summary>
    /// <value>The token value as a span of characters.</value>
    public ReadOnlySpan<char> ValueSpan { get; }

    /// <summary>
    ///     Gets the token value.
    /// </summary>
    /// <value>The token value.</value>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Type}: {Value}";
    }
}
