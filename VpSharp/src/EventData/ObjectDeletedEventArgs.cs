using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.ObjectDeleted" />.
/// </summary>
public sealed class ObjectDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ObjectDeletedEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The avatar responsible for the object being deleted.</param>
    /// <param name="objectId">The ID of the deleted object.</param>
    /// <param name="deletedObject">The deleted object.</param>
    public ObjectDeletedEventArgs(IAvatar avatar, int objectId, VirtualParadiseObject deletedObject)
    {
        Avatar = avatar;
        ObjectId = objectId;
        DeletedObject = deletedObject;
    }

    /// <summary>
    ///     Gets the avatar responsible for the object being deleted.
    /// </summary>
    /// <value>The avatar responsible for the object being deleted.</value>
    public IAvatar Avatar { get; }

    /// <summary>
    ///     Gets the object which was deleted.
    /// </summary>
    /// <value>The object which was deleted.</value>
    /// <remarks>
    ///     This value may be <see langword="null" /> if the client did not have the object cached prior to the deletion.
    ///     <see cref="ObjectId" /> will always be assigned.
    /// </remarks>
    /// <seealso cref="ObjectId" />
    public VirtualParadiseObject DeletedObject { get; }

    /// <summary>
    ///     Gets the ID of the object which was deleted.
    /// </summary>
    /// <value>The ID of the object which was deleted.</value>
    /// <remarks>
    ///     This value will always be assigned, regardless of whether or not the client had previously cached the object.
    /// </remarks>
    /// <seealso cref="DeletedObject" />
    public int ObjectId { get; }
}
