using System.Drawing;
using VpSharp.Building.Annotations;
using VpSharp.Building.Commands.Converters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>color</c> command.
/// </summary>
[Command("color")]
public sealed class ColorCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the color value.
    /// </summary>
    /// <value>The color value.</value>
    [Parameter(0)]
    public Color Color { get; set; } = Color.Transparent;
}
