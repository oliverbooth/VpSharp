namespace VpSharp.Building.Serialization;

/// <summary>
///     Represents a token type.
/// </summary>
public enum TokenType
{
    /// <summary>
    ///     No token type.
    /// </summary>
    None,

    /// <summary>
    ///     Number token type.
    /// </summary>
    Number,

    /// <summary>
    ///     Text token type.
    /// </summary>
    Text,

    /// <summary>
    ///     String token type.
    /// </summary>
    String,

    /// <summary>
    ///     Property token type.
    /// </summary>
    PropertyName
}
