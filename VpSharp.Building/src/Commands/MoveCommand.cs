using System.Numerics;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>move</c> command.
/// </summary>
[Command("move")]
public sealed class MoveCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the move value.
    /// </summary>
    /// <value>The move value.</value>
    public Vector3 Movement { get; set; } = Vector3.Zero;
}
