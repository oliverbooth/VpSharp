using VpSharp.Building.Annotations;
using VpSharp.Building.Serialization.CommandConverters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>teleport</c> command.
/// </summary>
[Command("teleport", ConverterType = typeof(TeleportCommandConverter))]
public sealed class TeleportCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the target coordinates.
    /// </summary>
    /// <value>The target coordinates.</value>
    [Parameter(0)]
    public Coordinates Coordinates { get; set; }
}
