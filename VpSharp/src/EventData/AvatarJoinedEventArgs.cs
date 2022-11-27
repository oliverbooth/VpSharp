using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.AvatarJoined" />.
/// </summary>
public sealed class AvatarJoinedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AvatarJoinedEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The newly-joined avatar.</param>
    public AvatarJoinedEventArgs(VirtualParadiseAvatar avatar)
    {
        Avatar = avatar;
    }

    /// <summary>
    ///     Gets the newly-joined avatar.
    /// </summary>
    /// <value>The newly-joined avatar.</value>
    public VirtualParadiseAvatar Avatar { get; }
}