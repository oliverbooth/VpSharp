using System.ComponentModel;

namespace VpSharp.Building;

/// <summary>
///     An enumeration of light effects that can be used on the <c>lightfx</c> property on the <c>light</c> command.
/// </summary>
public enum LightEffect
{
    /// <summary>
    ///     No light effect.
    /// </summary>
    [Description("No light effect.")] None,

    /// <summary>
    ///     Blink light effect.
    /// </summary>
    [Description("Blink light effect.")] Blink,

    /// <summary>
    ///     Fade in light effect.
    /// </summary>
    [Description("Fade in light effect.")] FadeIn,

    /// <summary>
    ///     Fade out light effect.
    /// </summary>
    [Description("Fade out light effect.")]
    FadeOut,

    /// <summary>
    ///     Fire light effect.
    /// </summary>
    [Description("Fire light effect.")] Fire,

    /// <summary>
    ///     Pulse light effect.
    /// </summary>
    [Description("Pulse light effect.")] Pulse,

    /// <summary>
    ///     Rainbow light effect.
    /// </summary>
    [Description("Rainbow light effect.")] Rainbow
}
