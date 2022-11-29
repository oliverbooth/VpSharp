using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.AvatarLeft" />.
/// </summary>
public sealed class AvatarLeftEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AvatarLeftEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The newly-departed avatar.</param>
    public AvatarLeftEventArgs(VirtualParadiseAvatar avatar)
    {
        Avatar = avatar;
    }

    /// <summary>
    ///     Gets the newly-departed avatar.
    /// </summary>
    /// <value>The newly-departed avatar.</value>
    public VirtualParadiseAvatar Avatar { get; }
}
