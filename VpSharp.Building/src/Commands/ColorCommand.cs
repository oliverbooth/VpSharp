using System.Drawing;
using VpSharp.Building.Commands.Converters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>color</c> command.
/// </summary>
[Command("color", ConverterType = typeof(ColorCommandConverter))]
public sealed class ColorCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the color value.
    /// </summary>
    /// <value>The color value.</value>
    public Color Color { get; set; } = Color.Transparent;
}
