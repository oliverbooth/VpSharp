using VpSharp.Entities;
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

    /// <summary>
    ///     Accepts this invite request.
    /// </summary>
    /// <param name="suppressTeleport">
    ///     If <see langword="true" />, the client's avatar will not teleport to the requested location automatically.
    /// </param>
    public Task AcceptAsync(bool suppressTeleport = false)
    {
        lock (_client.Lock)
        {
            Native.vp_invite_accept(_client.NativeInstanceHandle, _requestId);
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
            Native.vp_invite_decline(_client.NativeInstanceHandle, _requestId);
        }

        return Task.CompletedTask;
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
    public override int GetHashCode() => HashCode.Combine(_client, _requestId);

    public static bool operator ==(InviteRequest left, InviteRequest right) => Equals(left, right);

    public static bool operator !=(InviteRequest left, InviteRequest right) => !Equals(left, right);
}