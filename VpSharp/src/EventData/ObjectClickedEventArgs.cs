using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.ObjectClicked" />.
/// </summary>
public sealed class ObjectClickedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ObjectClickedEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The avatar responsible for the click.</param>
    /// <param name="clickedObject">The clicked object.</param>
    /// <param name="clickPoint">The click point.</param>
    public ObjectClickedEventArgs(
        IAvatar avatar,
        VirtualParadiseObject clickedObject,
        Vector3d clickPoint)
    {
        Avatar = avatar;
        ClickedObject = clickedObject;
        ClickPoint = clickPoint;
    }

    /// <summary>
    ///     Gets the avatar responsible for the click.
    /// </summary>
    /// <value>The avatar responsible for the click.</value>
    public IAvatar Avatar { get; }

    /// <summary>
    ///     Gets the clicked object.
    /// </summary>
    /// <value>The clicked object.</value>
    public VirtualParadiseObject ClickedObject { get; }

    /// <summary>
    ///     Gets the point at which the avatar clicked the object.
    /// </summary>
    /// <value>The click point.</value>
    public Vector3d ClickPoint { get; }
}
