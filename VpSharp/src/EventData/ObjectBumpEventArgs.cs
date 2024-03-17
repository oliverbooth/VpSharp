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
    /// <param name="bumpedObject">The bumped object.</param>
    /// <param name="phase">The bump phase.</param>
    public ObjectBumpEventArgs(IAvatar avatar, VirtualParadiseObject bumpedObject, BumpPhase phase)
    {
        Avatar = avatar;
        BumpedObject = bumpedObject;
        Phase = phase;
    }

    /// <summary>
    ///     Gets the avatar responsible for the bump.
    /// </summary>
    /// <value>The avatar responsible for the bump.</value>
    public IAvatar Avatar { get; }

    /// <summary>
    ///     Gets the object to which this event pertains.
    /// </summary>
    /// <value>The object to which this event pertains.</value>
    public VirtualParadiseObject BumpedObject { get; }

    /// <summary>
    ///     Gets the bump phase.
    /// </summary>
    /// <value>The bump phase.</value>
    public BumpPhase Phase { get; }
}
