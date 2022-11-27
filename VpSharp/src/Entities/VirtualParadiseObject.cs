using System.Numerics;
using VpSharp.Exceptions;
using VpSharp.Extensions;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using static VpSharp.Internal.Native;

namespace VpSharp.Entities;

/// <summary>
///     Represents the base class for all in-world objects.
/// </summary>
public abstract class VirtualParadiseObject : IEquatable<VirtualParadiseObject>
{
    protected internal VirtualParadiseObject(VirtualParadiseClient client, int id)
    {
        Client = client;
        Id = id;
    }

    /// <summary>
    ///     Gets the unique ID of this object.
    /// </summary>
    /// <value>The unique ID of this object.</value>
    public int Id { get; }

    /// <summary>
    ///     Gets the location of this object.
    /// </summary>
    /// <value>The location of this object.</value>
    public Location Location { get; internal set; }

    /// <summary>
    ///     Gets the owner of this object.
    /// </summary>
    /// <value>The owner of this object.</value>
    public VirtualParadiseUser Owner { get; internal set; } = null!;

    private protected VirtualParadiseClient Client { get; }

    /// <summary>
    ///     Returns a value indicating whether the two given objects are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true" /> if the two objects are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(VirtualParadiseObject left, VirtualParadiseObject right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Returns a value indicating whether the two given objects are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true" /> if the two objects are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(VirtualParadiseObject left, VirtualParadiseObject right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    ///     Performs a bump on this object.
    /// </summary>
    /// <param name="phase">
    ///     The bump phase. If this value is <see langword="null" />, both <see cref="BumpPhase.Begin" /> and
    ///     <see cref="BumpPhase.End" /> are sent in succession.
    /// </param>
    /// <param name="target">
    ///     The target avatar to receive the event. If this value is <see langword="null" />, the bump will be broadcast to
    ///     all avatars in the world.
    /// </param>
    public async Task BumpAsync(BumpPhase? phase = null, VirtualParadiseAvatar? target = null)
    {
        int session = target?.Session ?? 0;

        ValueTask SendBegin()
        {
            lock (Client.Lock)
            {
                vp_object_bump_begin(Client.NativeInstanceHandle, Id, session);
            }

            return ValueTask.CompletedTask;
        }

        ValueTask SendEnd()
        {
            lock (Client.Lock)
            {
                vp_object_bump_end(Client.NativeInstanceHandle, Id, session);
            }

            return ValueTask.CompletedTask;
        }

        switch (phase)
        {
            case BumpPhase.Begin:
                await SendBegin();
                break;

            case BumpPhase.End:
                await SendEnd();
                break;

            case null:
                await SendBegin();
                await SendEnd();
                break;
        }
    }

    /// <summary>
    ///     Clicks the object.
    /// </summary>
    /// <param name="position">The position at which to click the object.</param>
    /// <param name="target">
    ///     The target avatar which will receive the event, or <see langword="null" /> to broadcast to every avatar.
    /// </param>
    /// <exception cref="InvalidOperationException"><paramref name="target" /> is the client's current avatar.</exception>
    public Task ClickAsync(Vector3d? position = null, VirtualParadiseAvatar? target = null)
    {
        if (target == Client.CurrentAvatar)
        {
            ThrowHelper.ThrowCannotUseSelfException();
        }

        lock (Client.Lock)
        {
            int session = target?.Session ?? 0;
            (float x, float y, float z) = (Vector3) (position ?? Vector3d.Zero);

            vp_object_click(Client.NativeInstanceHandle, Id, session, x, y, z);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Deletes this object.
    /// </summary>
    /// <exception cref="InvalidOperationException">The client is not connected to a world.</exception>
    /// <exception cref="ObjectNotFoundException">The object does not exist.</exception>
    public Task DeleteAsync()
    {
        lock (Client.Lock)
        {
            var reason = (ReasonCode) vp_object_delete(Client.NativeInstanceHandle, Id);

            switch (reason)
            {
                case ReasonCode.NotInWorld:
                    return Task.FromException(ThrowHelper.NotInWorldException());

                case ReasonCode.ObjectNotFound:
                    return Task.FromException(ThrowHelper.ObjectNotFoundException());
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Returns a value indicating whether this object and another object are equal.
    /// </summary>
    /// <param name="other">The object to compare with this instance.</param>
    /// <returns><see langword="true" /> if the two objects are equal; otherwise, <see langword="false" />.</returns>
    public bool Equals(VirtualParadiseObject? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Location.World.Equals(other.Location.World) && Id == other.Id;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((VirtualParadiseObject) obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return HashCode.Combine(Location.World, Id);
    }

    protected internal virtual void ExtractFromOther(VirtualParadiseObject virtualParadiseObject)
    {
        Location = virtualParadiseObject.Location;
        Owner = virtualParadiseObject.Owner;
    }

    protected internal virtual void ExtractFromInstance(IntPtr handle)
    {
        var data = Span<byte>.Empty;
        IntPtr dataPtr = vp_data(handle, DataAttribute.ObjectData, out int length);

        if (length > 0)
        {
            unsafe
            {
                data = new Span<byte>(dataPtr.ToPointer(), length);
            }
        }

        ExtractFromData(data);
    }

    protected virtual void ExtractFromData(ReadOnlySpan<byte> data)
    {
    }
}
