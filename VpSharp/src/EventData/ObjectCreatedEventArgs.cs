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
    /// <param name="createdObject">The created object.</param>
    public ObjectCreatedEventArgs(Avatar avatar, VirtualParadiseObject createdObject)
    {
        Avatar = avatar;
        CreatedObject = createdObject;
    }

    /// <summary>
    ///     Gets the avatar responsible for the object being created.
    /// </summary>
    /// <value>The avatar responsible for the object being created.</value>
    public Avatar Avatar { get; }

    /// <summary>
    ///     Gets the object which was created.
    /// </summary>
    /// <value>The object which was created.</value>
    public VirtualParadiseObject CreatedObject { get; }
}
