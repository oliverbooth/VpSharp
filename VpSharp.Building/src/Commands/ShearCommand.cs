using VpSharp.Building.Commands.Converters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>shear</c> command.
/// </summary>
[Command("shear", ConverterType = typeof(ShearCommandConverter))]
public sealed class ShearCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the positive shear value.
    /// </summary>
    /// <value>The positive shear value.</value>
    public Vector3d PositiveShear { get; set; }

    /// <summary>
    ///     Gets or sets the negative shear value.
    /// </summary>
    /// <value>The negative shear value.</value>
    public Vector3d NegativeShear { get; set; }
}
