using VpSharp.Building.Annotations;
using VpSharp.Building.Commands.Converters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>scale</c> command.
/// </summary>
[Command("scale")]
public sealed class ScaleCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the scale factor.
    /// </summary>
    /// <value>The scale factor.</value>
    public Vector3d Scale
    {
        get => new(X, Y, Z);
        set
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
        }
    }

    [Parameter(0)] private double X { get; set; } = 1.0;
    [Parameter(1, IsOptional = true)] private double Y { get; set; } = 1.0;
    [Parameter(2, IsOptional = true)] private double Z { get; set; } = 1.0;
}
