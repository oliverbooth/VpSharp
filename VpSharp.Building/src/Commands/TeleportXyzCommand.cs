using VpSharp.Building.Annotations;
using VpSharp.Building.Serialization.CommandConverters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>teleportxyz</c> command.
/// </summary>
[Command("teleportxyz", ConverterType = typeof(TeleportXyzCommandConverter))]
public sealed class TeleportXyzCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the facing direction.
    /// </summary>
    /// <value>The facing direction.</value>
    [Parameter(1, IsOptional = true)]
    public double Yaw { get; set; }

    /// <summary>
    ///     Gets or sets the teleport destination.
    /// </summary>
    /// <value>The teleport destination.</value>
    [Parameter(0)]
    public Vector3d Destination { get; set; }
}
