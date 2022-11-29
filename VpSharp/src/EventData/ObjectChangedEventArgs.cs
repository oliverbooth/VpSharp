using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.ObjectChanged" />.
/// </summary>
public sealed class ObjectChangedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ObjectChangedEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The avatar which changed the object.</param>
    /// <param name="objectBefore">The state of the object prior to the change.</param>
    /// <param name="virtualParadiseObject">The object which was changed, containing updated values.</param>
    public ObjectChangedEventArgs(
        VirtualParadiseAvatar avatar,
        VirtualParadiseObject objectBefore,
        VirtualParadiseObject virtualParadiseObject)
    {
        Avatar = avatar;
        Object = virtualParadiseObject;
        ObjectBefore = objectBefore;
    }

    /// <summary>
    ///     Gets the avatar which changed the object.
    /// </summary>
    /// <value>The avatar which changed the object.</value>
    public VirtualParadiseAvatar Avatar { get; }

    /// <summary>
    ///     Gets the object which was changed.
    /// </summary>
    /// <value>The object which was changed.</value>
    /// <remarks>This instance will contain the updated values of the object.</remarks>
    public VirtualParadiseObject Object { get; }

    /// <summary>
    ///     Gets the state of the object prior to the change.
    /// </summary>
    /// <value>
    ///     The state of the object prior to the change. This value may be <see langword="null" /> if the client did not
    ///     previously have the object cached.
    /// </value>
    public VirtualParadiseObject ObjectBefore { get; }
}
