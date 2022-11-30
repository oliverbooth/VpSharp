namespace VpSharp;

#pragma warning disable CA1027

/// <summary>
///     An enumeration of fog modes.
/// </summary>
public enum FogMode
{
    /// <summary>
    ///     None.
    /// </summary>
    None,

    /// <summary>
    ///     Linear fog mode.
    /// </summary>
    Linear,

    /// <summary>
    ///     Exponential fog mode.
    /// </summary>
    Exponential,

    /// <summary>
    ///     Exponential squared fog mode.
    /// </summary>
    Exponential2,

    /// <summary>
    ///     Exponential fog mode. This value is used for serialization purposes.
    /// </summary>
    Exp = Exponential,

    /// <summary>
    ///     Exponential squared fog mode. This value is used for serialization purposes.
    /// </summary>
    Exp2 = Exponential2
}
