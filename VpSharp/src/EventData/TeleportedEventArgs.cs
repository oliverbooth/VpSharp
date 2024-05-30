using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.Teleported" />.
/// </summary>
public sealed class TeleportedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TeleportedEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The avatar which initiated the teleport.</param>
    /// <param name="location">The target location of the teleport.</param>
    public TeleportedEventArgs(VirtualParadiseAvatar avatar, Location location)
    {
        Avatar = avatar;
        Location = location;
    }

    /// <summary>
    ///     Gets the avatar which initiated the teleport.
    /// </summary>
    /// <value>The avatar which initiated the teleport.</value>
    public VirtualParadiseAvatar Avatar { get; }

    /// <summary>
    ///     Gets the target location of the teleport.
    /// </summary>
    /// <value>The target location of the teleport.</value>
    public Location Location { get; }
}
