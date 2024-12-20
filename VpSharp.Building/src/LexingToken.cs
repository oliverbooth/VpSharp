namespace VpSharp.Building;

/// <summary>
///     Represents a token.
/// </summary>
internal struct LexingToken
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LexingToken" /> structure.
    /// </summary>
    /// <param name="type">The token type.</param>
    /// <param name="value">The token value.</param>
    public LexingToken(LexingTokenType type, ReadOnlySpan<char> value)
    {
        Type = type;
        Value = value.ToString();
    }

    /// <summary>
    ///     Gets the token type.
    /// </summary>
    /// <value>The token type.</value>
    public LexingTokenType Type { get; }

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
