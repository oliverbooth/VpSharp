using System.Drawing;
using VpSharp.Building.Annotations;
using VpSharp.Building.Serialization.ValueConverters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>light</c> command.
/// </summary>
[Command("light")]
public sealed class LightCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the angle of the light.
    /// </summary>
    /// <value>The angle of the light.</value>
    [Property("angle")]
    public double Angle { get; set; } = 45.0;

    /// <summary>
    ///     Gets or sets the brightness of the light.
    /// </summary>
    /// <value>The brightness of the light.</value>
    [Property("brightness")]
    public double Brightness { get; set; } = 0.5;

    /// <summary>
    ///     Gets or sets the color of the light.
    /// </summary>
    /// <value>The color of the light.</value>
    [Property("color")]
    public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Gets or sets the effect of the light.
    /// </summary>
    /// <value>The effect of the light.</value>
    [Property("fx")]
    public LightEffect Effect { get; set; } = LightEffect.None;

    /// <summary>
    ///     Gets or sets the maximum distance of the light in meters.
    /// </summary>
    /// <value>The maximum distance of the light in meters.</value>
    [Property("maxdist")]
    public double MaxDistance { get; set; } = 1000.0;

    /// <summary>
    ///     Gets or sets the radius of the light in meters.
    /// </summary>
    /// <value>The radius of the light in meters.</value>
    [Property("radius")]
    public double Radius { get; set; } = 10.0;

    /// <summary>
    ///     Gets or sets the interval of the light animation.
    /// </summary>
    /// <value>The interval of the light animation.</value>
    [Property("time")]
    [ValueConverter(typeof(TimeSpanToSecondsValueConverter))]
    public TimeSpan Time { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    ///     Gets or sets the type of the light.
    /// </summary>
    /// <value>The type of the light.</value>
    [Property("type")]
    public LightType Type { get; set; } = LightType.Point;
}
