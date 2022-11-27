﻿using System.Numerics;
using VpSharp.Extensions;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using static VpSharp.Internal.Native;

namespace VpSharp.Entities;

/// <summary>
///     Represents an avatar within a world.
/// </summary>
public sealed class VirtualParadiseAvatar : IEquatable<VirtualParadiseAvatar>
{
    private readonly VirtualParadiseClient _client;

    internal VirtualParadiseAvatar(VirtualParadiseClient client, int session)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        Session = session;
    }

    /// <summary>
    ///     Gets the details of the application this avatar is using.
    /// </summary>
    /// <value>The avatar's application details.</value>
    public Application Application { get; internal set; } = null!;

    /// <summary>
    ///     Gets a value indicating whether this avatar is a bot.
    /// </summary>
    /// <value><see langword="true" /> if this avatar is a bot; otherwise, <see langword="false" />.</value>
    public bool IsBot => Name is {Length: > 1} name && name[0] == '[' && name[^1] == ']';

    /// <summary>
    ///     Gets the location of this avatar.
    /// </summary>
    /// <value>The location of this avatar.</value>
    public Location Location { get; internal set; }

    /// <summary>
    ///     Gets the name of this avatar.
    /// </summary>
    /// <value>The name of this avatar.</value>
    public string Name { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the session ID of this avatar.
    /// </summary>
    /// <value>The session ID.</value>
    public int Session { get; internal set; }

    /// <summary>
    ///     Gets the type of this avatar.
    /// </summary>
    /// <value>The type of this avatar.</value>
    public int Type { get; internal set; }

    /// <summary>
    ///     Gets the user associated with this avatar.
    /// </summary>
    /// <value>The user.</value>
    public VirtualParadiseUser User { get; internal set; } = null!;

    /// <summary>
    ///     Determines if two <see cref="VirtualParadiseAvatar" /> instances are equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool operator ==(VirtualParadiseAvatar? left, VirtualParadiseAvatar? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Determines if two <see cref="VirtualParadiseAvatar" /> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool operator !=(VirtualParadiseAvatar? left, VirtualParadiseAvatar? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    ///     Determines if two <see cref="VirtualParadiseAvatar" /> instances are equal.
    /// </summary>
    /// <param name="other">The other instance.</param>
    /// <returns>
    ///     <see langword="true" /> if this instance is equal to <paramref name="other" />; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(VirtualParadiseAvatar? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Session == other.Session && User.Equals(other.User);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is VirtualParadiseAvatar other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return HashCode.Combine(Session, User);
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    /// <summary>
    ///     Clicks this avatar.
    /// </summary>
    /// <param name="clickPoint">The position at which the avatar should be clicked.</param>
    /// <exception cref="InvalidOperationException">
    ///     <para>The action cannot be performed on the client's current avatar.</para>
    ///     -or-
    ///     <para>An attempt was made to click an avatar outside of a world.</para>
    /// </exception>
    public Task ClickAsync(Vector3d? clickPoint = null)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        if (this == _client.CurrentAvatar)
            return Task.FromException(ThrowHelper.CannotUseSelfException());

        clickPoint ??= Location.Position;
        (double x, double y, double z) = clickPoint.Value;

        lock (_client.Lock)
        {
            IntPtr handle = _client.NativeInstanceHandle;

            vp_double_set(handle, FloatAttribute.ClickHitX, x);
            vp_double_set(handle, FloatAttribute.ClickHitY, y);
            vp_double_set(handle, FloatAttribute.ClickHitZ, z);

            var reason = (ReasonCode) vp_avatar_click(handle, Session);
            if (reason == ReasonCode.NotInWorld)
                return Task.FromException(ThrowHelper.NotInWorldException());
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Sends a URI to this avatar.
    /// </summary>
    /// <param name="uri">The URI to send.</param>
    /// <param name="target">The URL target. See <see cref="UriTarget" /> for more information.</param>
    /// <exception cref="InvalidOperationException">The action cannot be performed on the client's current avatar.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="uri" /> is <see langword="null" />.</exception>
    public Task SendUriAsync(Uri uri, UriTarget target = UriTarget.Browser)
    {
        ArgumentNullException.ThrowIfNull(uri);

        // ReSharper disable once InconsistentlySynchronizedField
        if (this == _client.CurrentAvatar)
            return Task.FromException(ThrowHelper.CannotUseSelfException());

        lock (_client.Lock)
        {
            vp_url_send(_client.NativeInstanceHandle, Session, uri.ToString(), target);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Teleports the avatar to another world.
    /// </summary>
    /// <param name="world">The name of the world to which this avatar should be teleported.</param>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    public Task TeleportAsync(VirtualParadiseWorld world, Vector3d position)
    {
        return TeleportAsync(world.Name, position, Quaternion.Identity);
    }

    /// <summary>
    ///     Teleports the avatar to another world.
    /// </summary>
    /// <param name="world">The name of the world to which this avatar should be teleported.</param>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    /// <param name="rotation">The rotation to which this avatar should be teleported.</param>
    public Task TeleportAsync(VirtualParadiseWorld world, Vector3d position, Quaternion rotation)
    {
        return TeleportAsync(world.Name, position, rotation);
    }

    /// <summary>
    ///     Teleports the avatar to another world.
    /// </summary>
    /// <param name="world">The name of the world to which this avatar should be teleported.</param>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    public Task TeleportAsync(string world, Vector3d position)
    {
        return TeleportAsync(world, position, Quaternion.Identity);
    }

    /// <summary>
    ///     Teleports the avatar to another world.
    /// </summary>
    /// <param name="world">The name of the world to which this avatar should be teleported.</param>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    /// <param name="rotation">The rotation to which this avatar should be teleported.</param>
    public async Task TeleportAsync(string world, Vector3d position, Quaternion rotation)
    {
        // ReSharper disable InconsistentlySynchronizedField
        bool isSelf = this == _client.CurrentAvatar;
        bool isNewWorld = world != Location.World.Name;

        if (world == Location.World.Name)
        {
            world = string.Empty;
        }

        if (isSelf && isNewWorld)
        {
            await _client.EnterAsync(world);
        }

        IntPtr handle = _client.NativeInstanceHandle;

        if (this == _client.CurrentAvatar)
        {
            if (!string.IsNullOrWhiteSpace(world))
            {
                await _client.EnterAsync(world);
            }

            // state change self
            lock (_client.Lock)
            {
                (double x, double y, double z) = position;
                (double pitch, double yaw, double _) = rotation.ToEulerAngles(false);

                vp_double_set(handle, FloatAttribute.MyX, x);
                vp_double_set(handle, FloatAttribute.MyY, y);
                vp_double_set(handle, FloatAttribute.MyZ, z);
                vp_double_set(handle, FloatAttribute.MyPitch, pitch);
                vp_double_set(handle, FloatAttribute.MyYaw, yaw);

                var reason = (ReasonCode) vp_state_change(handle);
                if (reason == ReasonCode.NotInWorld)
                    ThrowHelper.ThrowNotInWorldException();
            }
        }
        else
        {
            lock (_client.Lock)
            {
                (float x, float y, float z) = (Vector3) position;
                (float pitch, float yaw, float _) = (Vector3) rotation.ToEulerAngles(false);

                var reason = (ReasonCode) vp_teleport_avatar(handle, Session, world, x, y, z, yaw, pitch);
                if (reason == ReasonCode.NotInWorld)
                    ThrowHelper.ThrowNotInWorldException();
            }
        }

        Location = new Location(new VirtualParadiseWorld(_client, world), position, rotation);
        // ReSharper restore InconsistentlySynchronizedField
    }

    /// <summary>
    ///     Teleports the avatar to a new position within the current world.
    /// </summary>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    public Task TeleportAsync(Vector3d position)
    {
        return TeleportAsync(Location with {Position = position});
    }

    /// <summary>
    ///     Teleports the avatar to a new position and rotation within the current world.
    /// </summary>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    /// <param name="rotation">The rotation to which this avatar should be teleported</param>
    public Task TeleportAsync(Vector3d position, Quaternion rotation)
    {
        return TeleportAsync(Location with {Position = position, Rotation = rotation});
    }

    /// <summary>
    ///     Teleports this avatar to a new location, which may or may not be a new world.
    /// </summary>
    /// <param name="location">The location to which this avatar should be teleported.</param>
    public Task TeleportAsync(Location location)
    {
        return TeleportAsync(location.World.Name, location.Position, location.Rotation);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Avatar [Session={Session}, Name={Name}]";
    }
}
