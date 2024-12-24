using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>teleportxyz</c> command.
/// </summary>
[Command("teleportxyz")]
public sealed class TeleportXyzCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the facing direction.
    /// </summary>
    /// <value>The facing direction.</value>
    [Parameter(3, IsOptional = true)]
    public double Yaw { get; set; }

    /// <summary>
    ///     Gets or sets the teleport destination.
    /// </summary>
    /// <value>The teleport destination.</value>
    public Vector3d Destination
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
