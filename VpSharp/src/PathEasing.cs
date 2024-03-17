using VpSharp.Entities;

namespace VpSharp;

/// <summary>
///     An enumeration of easings for use with <see cref="Entities.Path" />.
/// </summary>
public enum PathEasing
{
    /// <summary>
    ///     Linear easing.
    /// </summary>
    Linear,

    /// <summary>
    ///     Cubic easing.
    /// </summary>
    Cubic,

    /// <summary>
    ///     Catmull-Rom easing.
    /// </summary>
    CatmullRom
}
