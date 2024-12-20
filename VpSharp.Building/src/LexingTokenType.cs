namespace VpSharp.Building;

/// <summary>
///     Represents a type of <see cref="LexingToken" />.
/// </summary>
internal enum LexingTokenType
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
    ///     Text token.
    /// </summary>
    Text,

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
