using System.Drawing;

namespace VpSharp.Entities;

/// <summary>
///     Represents a world in the Virtual Paradise universe.
/// </summary>
public sealed class VirtualParadiseWorld : IEquatable<VirtualParadiseWorld>
{
    /// <summary>
    ///     A world that represents no world in the universe.
    /// </summary>
    public static readonly VirtualParadiseWorld Nowhere = new(null!, "") {IsNowhere = true};

    private readonly VirtualParadiseClient _client;

    internal VirtualParadiseWorld(VirtualParadiseClient client, string name)
    {
        _client = client;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    ///     Gets the number of avatars currently in this world.
    /// </summary>
    /// <value>The number of avatars currently in this world.</value>
    public int AvatarCount { get; internal set; }

    /// <summary>
    ///     Gets the name of this world.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; internal set; }

    /// <summary>
    ///     Gets the settings for this world.
    /// </summary>
    /// <value>The settings for this world.</value>
    public WorldSettings Settings { get; internal set; } = new();

    /// <summary>
    ///     Gets the size of this world.
    /// </summary>
    /// <value>The size of this world.</value>
    public Size Size { get; internal set; }

    /// <summary>
    ///     Gets the state of this world.
    /// </summary>
    /// <value>The state of this world.</value>
    public WorldState State { get; internal set; } = WorldState.Unknown;

    internal bool IsNowhere { get; private init; }

    /// <summary>
    ///     Determines if two <see cref="VirtualParadiseWorld" /> instances are equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool operator ==(VirtualParadiseWorld? left, VirtualParadiseWorld? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Determines if two <see cref="VirtualParadiseWorld" /> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool operator !=(VirtualParadiseWorld? left, VirtualParadiseWorld? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    ///     Determines if two <see cref="VirtualParadiseWorld" /> instances are equal.
    /// </summary>
    /// <param name="other">The other instance.</param>
    /// <returns>
    ///     <see langword="true" /> if this instance is equal to <paramref name="other" />; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(VirtualParadiseWorld? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return IsNowhere == other.IsNowhere && Name == other.Name;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is VirtualParadiseWorld other && Equals(other));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return HashCode.Combine(IsNowhere, Name);
    }

    /// <summary>
    ///     Modifies the world settings globally.
    /// </summary>
    /// <param name="action">The builder which defines parameters to change.</param>
    /// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
    /// <exception cref="UnauthorizedAccessException">The client does not have permission to modify world settings.</exception>
    public async Task ModifyAsync(Action<WorldSettingsBuilder> action)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var builder = new WorldSettingsBuilder(_client);
        await Task.Run(() => action(builder)).ConfigureAwait(false);

        builder.SendChanges();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"World {Name}";
    }
}
