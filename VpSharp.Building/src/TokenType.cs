namespace VpSharp.Building;

/// <summary>
///     Represents a type of <see cref="Token" />.
/// </summary>
internal enum TokenType
{
    /// <summary>
    ///     No token.
    /// </summary>
    None,

    /// <summary>
    ///     String token.
    /// </summary>
    String,

    /// <summary>
    ///     Number token.
    /// </summary>
    Number,

    /// <summary>
    ///     Property token.
    /// </summary>
    Property,

    /// <summary>
    ///     Operator token.
    /// </summary>
    Operator,

    /// <summary>
    ///     EOF token.
    /// </summary>
    Eof
}