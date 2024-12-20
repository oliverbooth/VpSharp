using VpSharp.Building.Commands.Converters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>opacity</c> command.
/// </summary>
[Command("opacity", ConverterType = typeof(OpacityCommandConverter))]
public sealed class OpacityCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the opacity value.
    /// </summary>
    /// <value>The opacity value.</value>
    public double Opacity { get; set; } = 1.0;
}
