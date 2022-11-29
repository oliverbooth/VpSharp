using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.ObjectCreated" />.
/// </summary>
public sealed class ObjectCreatedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ObjectClickedEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The avatar responsible for the object being created.</param>
    /// <param name="virtualParadiseObject">The created object.</param>
    public ObjectCreatedEventArgs(VirtualParadiseAvatar avatar, VirtualParadiseObject virtualParadiseObject)
    {
        Avatar = avatar;
        Object = virtualParadiseObject;
    }

    /// <summary>
    ///     Gets the avatar responsible for the object being created.
    /// </summary>
    /// <value>The avatar responsible for the object being created.</value>
    public VirtualParadiseAvatar Avatar { get; }

    /// <summary>
    ///     Gets the object which was created.
    /// </summary>
    /// <value>The object which was created.</value>
    public VirtualParadiseObject Object { get; }
}
