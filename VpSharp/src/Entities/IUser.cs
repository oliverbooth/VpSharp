using VpSharp.Exceptions;

namespace VpSharp.Entities;

public interface IUser
{
    /// <summary>
    ///     Gets the email address of this user.
    /// </summary>
    /// <value>The email address of this user.</value>
    string EmailAddress { get; }

    /// <summary>
    ///     Gets the ID of this user.
    /// </summary>
    /// <value>The user's ID.</value>
    int Id { get; }

    /// <summary>
    ///     Gets the date and time at which this user was last online.
    /// </summary>
    /// <value>A <see cref="DateTimeOffset" /> representing the date and time this user was last online.</value>
    DateTimeOffset LastLogin { get; }

    /// <summary>
    ///     Gets the name of this user.
    /// </summary>
    /// <value>The user's name.</value>
    string Name { get; }

    /// <summary>
    ///     Gets the duration for which this user has been online.
    /// </summary>
    /// <value>A <see cref="TimeSpan" /> representing the duration for which this user has been online.</value>
    TimeSpan OnlineTime { get; }

    /// <summary>
    ///     Gets the date and time at which this user was registered.
    /// </summary>
    /// <value>A <see cref="DateTimeOffset" /> representing the date and time this user was registered.</value>
    DateTimeOffset RegistrationTime { get; }

    /// <summary>
    ///     Determines if two <see cref="User" /> instances are equal.
    /// </summary>
    /// <param name="other">The other instance.</param>
    /// <returns>
    ///     <see langword="true" /> if this instance is equal to <paramref name="other" />; otherwise, <see langword="false" />.
    /// </returns>
    bool Equals(User? other);

    /// <summary>
    ///     Invites this user to a specified location.
    /// </summary>
    /// <param name="location">
    ///     The invitation location. If <see langword="null" />, the client's current location is used.
    /// </param>
    Task<InviteResponse> InviteAsync(Location? location = null);

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
    Task<JoinResult> JoinAsync(bool suppressTeleport = false);
}
