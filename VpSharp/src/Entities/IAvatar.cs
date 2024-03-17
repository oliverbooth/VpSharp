using System.Drawing;

namespace VpSharp.Entities;

public interface IAvatar
{
    /// <summary>
    ///     Gets the details of the application this avatar is using.
    /// </summary>
    /// <value>The avatar's application details.</value>
    Application Application { get; }

    /// <summary>
    ///     Gets a value indicating whether this avatar is a bot.
    /// </summary>
    /// <value><see langword="true" /> if this avatar is a bot; otherwise, <see langword="false" />.</value>
    bool IsBot { get; }

    /// <summary>
    ///     Gets the location of this avatar.
    /// </summary>
    /// <value>The location of this avatar.</value>
    Location Location { get; }

    /// <summary>
    ///     Gets the name of this avatar.
    /// </summary>
    /// <value>The name of this avatar.</value>
    string Name { get; }

    /// <summary>
    ///     Gets the session ID of this avatar.
    /// </summary>
    /// <value>The session ID.</value>
    int Session { get; }

    /// <summary>
    ///     Gets the type of this avatar.
    /// </summary>
    /// <value>The type of this avatar.</value>
    int Type { get; }

    /// <summary>
    ///     Gets the user ID associated with this avatar.
    /// </summary>
    /// <value>The user ID.</value>
    int UserId { get; }

    /// <summary>
    ///     Clicks this avatar.
    /// </summary>
    /// <param name="clickPoint">The position at which the avatar should be clicked.</param>
    /// <exception cref="InvalidOperationException">
    ///     <para>The action cannot be performed on the client's current avatar.</para>
    ///     -or-
    ///     <para>An attempt was made to click an avatar outside of a world.</para>
    /// </exception>
    Task ClickAsync(Vector3d? clickPoint = null);

    /// <summary>
    ///     Determines if two <see cref="Avatar" /> instances are equal.
    /// </summary>
    /// <param name="other">The other instance.</param>
    /// <returns>
    ///     <see langword="true" /> if this instance is equal to <paramref name="other" />; otherwise, <see langword="false" />.
    /// </returns>
    bool Equals(Avatar? other);

    /// <summary>
    ///     Gets the user associated with this avatar.
    /// </summary>
    /// <returns>The user.</returns>
    Task<IUser> GetUserAsync();

    /// <summary>
    ///     Sends a console message to the avatar with no name.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="fontStyle">The font style of the message.</param>
    /// <param name="color">The text color of the message.</param>
    /// <returns>The message which was sent.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     <para>An attempt was made to send a message while not connected to a world.</para>
    ///     -or-
    ///     <para>An attempt was made to send a message to an avatar that is not in the world.</para>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <para><paramref name="message" /> is empty, or consists of only whitespace.</para>
    ///     -or-
    ///     <para><paramref name="message" /> is too long to send.</para>
    /// </exception>
    Task<IConsoleMessage> SendMessageAsync(string message, FontStyle fontStyle, Color color);

    /// <summary>
    ///     Sends a console message to the avatar.
    /// </summary>
    /// <param name="name">The apparent author of the message.</param>
    /// <param name="message">The message to send.</param>
    /// <param name="fontStyle">The font style of the message.</param>
    /// <param name="color">The text color of the message.</param>
    /// <returns>The message which was sent.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     <para>An attempt was made to send a message while not connected to a world.</para>
    ///     -or-
    ///     <para>An attempt was made to send a message to an avatar that is not in the world.</para>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <para><paramref name="message" /> is empty, or consists of only whitespace.</para>
    ///     -or-
    ///     <para><paramref name="message" /> is too long to send.</para>
    /// </exception>
    /// <remarks>Passing <see langword="null" /> to <paramref name="name" /> will hide the name from the recipient.</remarks>
    Task<IConsoleMessage> SendMessageAsync(string? name, string message, FontStyle fontStyle, Color color);

    /// <summary>
    ///     Sends a URI to this avatar.
    /// </summary>
    /// <param name="uri">The URI to send.</param>
    /// <param name="target">The URL target. See <see cref="UriTarget" /> for more information.</param>
    /// <exception cref="InvalidOperationException">The action cannot be performed on the client's current avatar.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="uri" /> is <see langword="null" />.</exception>
    Task SendUriAsync(Uri uri, UriTarget target = UriTarget.Browser);

    /// <summary>
    ///     Modifies the world settings for this avatar.
    /// </summary>
    /// <param name="action">The builder which defines parameters to change.</param>
    /// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
    /// <exception cref="UnauthorizedAccessException">The client does not have permission to modify world settings.</exception>
    Task SendWorldSettings(Action<WorldSettingsBuilder> action);

    /// <summary>
    ///     Teleports the avatar to another world.
    /// </summary>
    /// <param name="world">The name of the world to which this avatar should be teleported.</param>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    /// <exception cref="ArgumentNullException"><paramref name="world" /> is <see langword="null" />.</exception>
    Task TeleportAsync(World world, Vector3d position);

    /// <summary>
    ///     Teleports the avatar to another world.
    /// </summary>
    /// <param name="world">The name of the world to which this avatar should be teleported.</param>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    /// <param name="rotation">The rotation to which this avatar should be teleported.</param>
    /// <exception cref="ArgumentNullException"><paramref name="world" /> is <see langword="null" />.</exception>
    Task TeleportAsync(World world, Vector3d position, Rotation rotation);

    /// <summary>
    ///     Teleports the avatar to another world.
    /// </summary>
    /// <param name="world">The name of the world to which this avatar should be teleported.</param>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    Task TeleportAsync(string world, Vector3d position);

    /// <summary>
    ///     Teleports the avatar to another world.
    /// </summary>
    /// <param name="world">The name of the world to which this avatar should be teleported.</param>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    /// <param name="rotation">The rotation to which this avatar should be teleported.</param>
    Task TeleportAsync(string world, Vector3d position, Rotation rotation);

    /// <summary>
    ///     Teleports the avatar to a new position within the current world.
    /// </summary>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    Task TeleportAsync(Vector3d position);

    /// <summary>
    ///     Teleports the avatar to a new position and rotation within the current world.
    /// </summary>
    /// <param name="position">The position to which this avatar should be teleported.</param>
    /// <param name="rotation">The rotation to which this avatar should be teleported</param>
    Task TeleportAsync(Vector3d position, Rotation rotation);

    /// <summary>
    ///     Teleports this avatar to a new location, which may or may not be a new world.
    /// </summary>
    /// <param name="location">The location to which this avatar should be teleported.</param>
    Task TeleportAsync(Location location);
}
