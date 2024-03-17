using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.AvatarMoved" />.
/// </summary>
public sealed class AvatarMovedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AvatarMovedEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The avatar whose type was changed.</param>
    /// <param name="locationAfter">The avatar's new location.</param>
    /// <param name="locationBefore">The avatar's old location.</param>
    public AvatarMovedEventArgs(Avatar avatar, Location locationAfter, Location? locationBefore)
    {
        Avatar = avatar;
        LocationAfter = locationAfter;
        LocationBefore = locationBefore;
    }

    /// <summary>
    ///     Gets the avatar whose location was changed.
    /// </summary>
    /// <value>The avatar whose location was changed.</value>
    public Avatar Avatar { get; }

    /// <summary>
    ///     Gets the avatar's location after the change.
    /// </summary>
    /// <value>The avatar's new location.</value>
    public Location LocationAfter { get; }

    /// <summary>
    ///     Gets the avatar's location before the change.
    /// </summary>
    /// <value>The avatar's old location.</value>
    public Location? LocationBefore { get; }
}
