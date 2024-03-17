using VpSharp.Entities;
using VpSharp.Exceptions;
using VpSharp.Internal;

namespace VpSharp;

/// <summary>
///     Represents a join request, which the client may either accept or decline.
/// </summary>
public sealed class JoinRequest : IEquatable<JoinRequest>
{
    private readonly VirtualParadiseClient _client;
    private readonly int _requestId;

    internal JoinRequest(VirtualParadiseClient client, int requestId, string name, User user)
    {
        Name = name;
        User = user;
        _client = client;
        _requestId = requestId;
    }

    /// <summary>
    ///     Gets the name of the avatar which sent the request.
    /// </summary>
    /// <value>The name of the avatar which sent the request.</value>
    public string Name { get; }

    /// <summary>
    ///     Gets the user which sent the request.
    /// </summary>
    /// <value>The user which sent the request.</value>
    public User User { get; }

    /// <summary>
    ///     Returns a value indicating whether two <see cref="JoinRequest" /> instances are equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns><see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(JoinRequest? left, JoinRequest? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Returns a value indicating whether two <see cref="JoinRequest" /> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns><see langword="true" /> if the two instances are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(JoinRequest? left, JoinRequest? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    ///     Accepts this join request.
    /// </summary>
    /// <param name="location">
    ///     Optional. The target location of the join. Defaults to the client's current avatar location.
    /// </param>
    public Task AcceptAsync(Location? location = null)
    {
        if (_client.CurrentAvatar is null)
        {
            ThrowHelper.ThrowNotInWorldException();
        }

        if (_client.CurrentAvatar is null)
        {
            throw new NotInWorldException();
        }

        location ??= _client.CurrentAvatar.Location;
        string worldName = location.Value.World.Name;
        (double x, double y, double z) = location.Value.Position;
        (double pitch, double yaw, double _) = location.Value.Rotation;

        lock (_client.Lock)
        {
            _ = NativeMethods.vp_join_accept(_client.NativeInstanceHandle, _requestId, worldName, x, y, z, (float)yaw, (float)pitch);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Declines this join request.
    /// </summary>
    public Task DeclineAsync()
    {
        lock (_client.Lock)
        {
            _ = NativeMethods.vp_join_decline(_client.NativeInstanceHandle, _requestId);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public bool Equals(JoinRequest? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return _requestId == other._requestId && _client.Equals(other._client);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is JoinRequest other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(_client, _requestId);
    }
}
