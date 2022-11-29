using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.AvatarClicked" />.
/// </summary>
public sealed class AvatarClickedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AvatarClickedEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The avatar responsible for the click.</param>
    /// <param name="clickedAvatar">The clicked avatar.</param>
    /// <param name="clickPoint">The click point.</param>
    public AvatarClickedEventArgs(VirtualParadiseAvatar avatar, VirtualParadiseAvatar clickedAvatar, Vector3d clickPoint)
    {
        Avatar = avatar;
        ClickedAvatar = clickedAvatar;
        ClickPoint = clickPoint;
    }

    /// <summary>
    ///     Gets the avatar responsible for the click.
    /// </summary>
    /// <value>The avatar responsible for the click.</value>
    public VirtualParadiseAvatar Avatar { get; }

    /// <summary>
    ///     Gets the clicked avatar.
    /// </summary>
    /// <value>The clicked avatar.</value>
    public VirtualParadiseAvatar ClickedAvatar { get; }

    /// <summary>
    ///     Gets the point at which the avatar clicked the object.
    /// </summary>
    /// <value>The click point.</value>
    public Vector3d ClickPoint { get; }
}
