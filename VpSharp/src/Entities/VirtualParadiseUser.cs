using System.Numerics;
using VpSharp.Exceptions;
using VpSharp.Extensions;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using static VpSharp.Internal.Native;

namespace VpSharp.Entities;

/// <summary>
///     Represents a Virtual Paradise user.
/// </summary>
public sealed class VirtualParadiseUser : IEquatable<VirtualParadiseUser>
{
    private readonly VirtualParadiseClient _client;

    internal VirtualParadiseUser(VirtualParadiseClient client, int id)
    {
        _client = client;
        Id = id;
    }

    /// <summary>
    ///     Gets the email address of this user.
    /// </summary>
    /// <value>The email address of this user.</value>
    public string EmailAddress { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the ID of this user.
    /// </summary>
    /// <value>The user's ID.</value>
    public int Id { get; internal set; }

    /// <summary>
    ///     Gets the date and time at which this user was last online.
    /// </summary>
    /// <value>A <see cref="DateTimeOffset" /> representing the date and time this user was last online.</value>
    public DateTimeOffset LastLogin { get; internal set; }

    /// <summary>
    ///     Gets the name of this user.
    /// </summary>
    /// <value>The user's name.</value>
    public string Name { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the duration for which this user has been online.
    /// </summary>
    /// <value>A <see cref="TimeSpan" /> representing the duration for which this user has been online.</value>
    public TimeSpan OnlineTime { get; internal set; }

    /// <summary>
    ///     Gets the date and time at which this user was registered.
    /// </summary>
    /// <value>A <see cref="DateTimeOffset" /> representing the date and time this user was registered.</value>
    public DateTimeOffset RegistrationTime { get; internal set; }

    /// <summary>
    ///     Determines if two <see cref="VirtualParadiseUser" /> instances are equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool operator ==(VirtualParadiseUser? left, VirtualParadiseUser? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Determines if two <see cref="VirtualParadiseUser" /> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool operator !=(VirtualParadiseUser? left, VirtualParadiseUser? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    ///     Determines if two <see cref="VirtualParadiseUser" /> instances are equal.
    /// </summary>
    /// <param name="other">The other instance.</param>
    /// <returns>
    ///     <see langword="true" /> if this instance is equal to <paramref name="other" />; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(VirtualParadiseUser? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is VirtualParadiseUser other && Equals(other));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return Id;
    }

    /// <summary>
    ///     Invites this user to a specified location.
    /// </summary>
    /// <param name="location">
    ///     The invitation location. If <see langword="null" />, the client's current location is used.
    /// </param>
    public async Task<InviteResponse> InviteAsync(Location? location = null)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        if (_client.CurrentAvatar is not { } avatar)
        {
            ThrowHelper.ThrowNotInWorldException();
            return (InviteResponse)(-1);
        }

        location ??= avatar.Location;
        TaskCompletionSource<ReasonCode> taskCompletionSource;

        lock (_client.Lock)
        {
            int reference = ObjectReferenceCounter.GetNextReference();
            taskCompletionSource = _client.AddInviteCompletionSource(reference);

            string world = location.Value.World.Name;
            (double x, double y, double z) = location.Value.Position;
            (double pitch, double yaw, _) = location.Value.Rotation.ToEulerAngles();

            vp_int_set(_client.NativeInstanceHandle, IntegerAttribute.ReferenceNumber, reference);
            vp_invite(_client.NativeInstanceHandle, Id, world, x, y, z, (float) yaw, (float) pitch);
        }

        ReasonCode reason = await taskCompletionSource.Task.ConfigureAwait(false);
        return reason switch
        {
            ReasonCode.Success => InviteResponse.Accepted,
            ReasonCode.InviteDeclined => InviteResponse.Declined,
            ReasonCode.Timeout => InviteResponse.TimeOut,
            ReasonCode.NoSuchUser => throw new UserNotFoundException($"Cannot invite non-existent user {Id}."),
            var _ => throw new InvalidOperationException(
                $"An error occurred trying to invite the user: {reason:D} ({reason:G})")
        };
    }

    /// <summary>
    ///     Sends a to join request to the user.
    /// </summary>
    /// <param name="suppressTeleport">
    ///     If <see langword="true" />, the client's avatar will not teleport to the requested location automatically.
    ///     Be careful, there is no way to retrieve
    /// </param>
    /// <returns>The result of the request.</returns>
    /// <exception cref="UserNotFoundException">This user is invalid and cannot be joined.</exception>
    /// <exception cref="InvalidOperationException">An unexpected error occurred trying to join the user.</exception>
    public async Task<JoinResult> JoinAsync(bool suppressTeleport = false)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        if (_client.CurrentAvatar is not { } avatar)
        {
            ThrowHelper.ThrowNotInWorldException();
            return default;
        }

        // ReSharper disable InconsistentlySynchronizedField
        nint handle = _client.NativeInstanceHandle;
        TaskCompletionSource<ReasonCode> taskCompletionSource;

        lock (_client.Lock)
        {
            int reference = ObjectReferenceCounter.GetNextReference();
            vp_int_set(handle, IntegerAttribute.ReferenceNumber, reference);
            vp_join(handle, Id);

            taskCompletionSource = _client.AddJoinCompletionSource(reference);
        }

        ReasonCode reason = await taskCompletionSource.Task.ConfigureAwait(false);
        Location? location = null;

        if (reason == ReasonCode.Success)
        {
            string worldName;
            double x, y, z;
            float yaw, pitch;

            lock (_client.Lock)
            {
                x = vp_double(handle, FloatAttribute.JoinX);
                y = vp_double(handle, FloatAttribute.JoinY);
                z = vp_double(handle, FloatAttribute.JoinZ);

                yaw = (float)vp_double(handle, FloatAttribute.JoinYaw);
                pitch = (float)vp_double(handle, FloatAttribute.JoinPitch);

                worldName = vp_string(handle, StringAttribute.JoinWorld);
            }

            var position = new Vector3d(x, y, z);
            var rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0);
            VirtualParadiseWorld world = (await _client.GetWorldAsync(worldName).ConfigureAwait(false))!;

            location = new Location(world, position, rotation);

            if (!suppressTeleport)
            {
                await avatar.TeleportAsync(location.Value).ConfigureAwait(false);
            }
        }

        JoinResponse response = reason switch
        {
            ReasonCode.Success => JoinResponse.Accepted,
            ReasonCode.JoinDeclined => JoinResponse.Declined,
            ReasonCode.Timeout => JoinResponse.TimeOut,
            ReasonCode.NoSuchUser => throw new UserNotFoundException($"Cannot join non-existent user {Id}."),
            var _ => throw new InvalidOperationException(
                $"An error occurred trying to join the user: {reason:D} ({reason:G})")
        };
        // ReSharper enable InconsistentlySynchronizedField

        return new JoinResult(response, location);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"User [Id={Id}, Name={Name}]";
    }
}
