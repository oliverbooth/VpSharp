using VpSharp.Entities;
using VpSharp.Exceptions;
using VpSharp.Internal;

namespace VpSharp;

/// <summary>
///     Represents an invite request, which the client may either accept or decline.
/// </summary>
public sealed class InviteRequest : IEquatable<InviteRequest>
{
    private readonly VirtualParadiseClient _client;
    private readonly int _requestId;

    internal InviteRequest(
        VirtualParadiseClient client,
        int requestId,
        string name,
        VirtualParadiseUser user,
        Location location)
    {
        Name = name;
        User = user;
        Location = location;

        _client = client;
        _requestId = requestId;
    }

    /// <summary>
    ///     Gets the target location of this invite.
    /// </summary>
    public Location Location { get; }

    /// <summary>
    ///     Gets the name of the avatar which sent the request.
    /// </summary>
    /// <value>The name of the avatar which sent the request.</value>
    public string Name { get; }

    /// <summary>
    ///     Gets the user which sent the request.
    /// </summary>
    /// <value>The user which sent the request.</value>
    public VirtualParadiseUser User { get; }

    /// <summary>
    ///     Returns a value indicating whether two <see cref="InviteRequest" /> instances are equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns><see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(InviteRequest? left, InviteRequest? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Returns a value indicating whether two <see cref="InviteRequest" /> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns><see langword="true" /> if the two instances are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(InviteRequest? left, InviteRequest? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    ///     Accepts this invite request.
    /// </summary>
    /// <param name="suppressTeleport">
    ///     If <see langword="true" />, the client's avatar will not teleport to the requested location automatically.
    /// </param>
    public Task AcceptAsync(bool suppressTeleport = false)
    {
        if (_client.CurrentAvatar is null)
        {
            throw new NotInWorldException();
        }

        lock (_client.Lock)
        {
            _ = Native.vp_invite_accept(_client.NativeInstanceHandle, _requestId);
        }

        if (suppressTeleport)
        {
            return Task.CompletedTask;
        }

        return _client.CurrentAvatar.TeleportAsync(Location);
    }

    /// <summary>
    ///     Declines this invite request.
    /// </summary>
    public Task DeclineAsync()
    {
        lock (_client.Lock)
        {
            _ = Native.vp_invite_decline(_client.NativeInstanceHandle, _requestId);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public bool Equals(InviteRequest? other)
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

        return obj is InviteRequest other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(_client, _requestId);
    }
}
