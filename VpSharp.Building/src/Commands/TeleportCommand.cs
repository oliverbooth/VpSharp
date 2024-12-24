namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>teleport</c> command.
/// </summary>
[Command("teleport")]
public sealed class TeleportCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the target object name.
    /// </summary>
    /// <value>The target object name.</value>
    public Coordinates Coordinates
    {
        get => Coordinates.Parse(RawArgumentString);
        set => RawArgumentString = value.ToString();
    }
}
