using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.ObjectBump" />.
/// </summary>
public sealed class ObjectBumpEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ObjectBumpEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The avatar.</param>
    /// <param name="virtualParadiseObject">The object.</param>
    /// <param name="phase">The bump phase.</param>
    public ObjectBumpEventArgs(VirtualParadiseAvatar avatar, VirtualParadiseObject virtualParadiseObject, BumpPhase phase)
    {
        Avatar = avatar;
        Object = virtualParadiseObject;
        Phase = phase;
    }

    /// <summary>
    ///     Gets the avatar responsible for the bump.
    /// </summary>
    /// <value>The avatar responsible for the bump.</value>
    public VirtualParadiseAvatar Avatar { get; }

    /// <summary>
    ///     Gets the object to which this event pertains.
    /// </summary>
    /// <value>The object to which this event pertains.</value>
    public VirtualParadiseObject Object { get; }

    /// <summary>
    ///     Gets the bump phase.
    /// </summary>
    /// <value>The bump phase.</value>
    public BumpPhase Phase { get; }
}