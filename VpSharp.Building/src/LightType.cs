using System.ComponentModel;

namespace VpSharp.Building;

/// <summary>
///     An enumeration of light types that can be used by <see cref="LightCommand" />.
/// </summary>
public enum LightType
{
    /// <summary>
    ///     Point light.
    /// </summary>
    [Description("Point light.")] Point,

    /// <summary>
    ///     Spot light.
    /// </summary>
    [Description("Spot light.")] Spot
}
