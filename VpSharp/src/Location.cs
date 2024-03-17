using VpSharp.Entities;

namespace VpSharp;

/// <summary>
///     Represents a location within the Virtual Paradise universe.
/// </summary>
public readonly struct Location : IEquatable<Location>
{
    /// <summary>
    ///     A location that represents nowhere in the universe.
    /// </summary>
    public static readonly Location Nowhere = new(Entities.World.Nowhere);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Location" /> struct.
    /// </summary>
    /// <param name="world">The world.</param>
    /// <param name="coordinates">
    ///     The coordinates which contains the position and rotation. The <see cref="Coordinates.World" /> property in this value
    ///     is ignored. Fetch the world using <see cref="VirtualParadiseClient.GetWorldAsync" />, and pass that into the
    ///     <paramref name="world" /> parameter.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="world" /> is <see langword="null" />.</exception>
    public Location(World world, in Coordinates coordinates)
    {
        World = world ?? throw new ArgumentNullException(nameof(world));
        Position = new Vector3d(coordinates.X, coordinates.Y, coordinates.Z);
        Rotation = Rotation.CreateFromTiltYawRoll(0, coordinates.Yaw, 0);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Location" /> struct.
    /// </summary>
    /// <param name="world">The world.</param>
    /// <param name="position">The position.</param>
    /// <param name="rotation">The rotation.</param>
    /// <exception cref="ArgumentNullException"><paramref name="world" /> is <see langword="null" />.</exception>
    public Location(World world, Vector3d position = default, Rotation rotation = default)
    {
        World = world ?? throw new ArgumentNullException(nameof(world));
        Position = position;
        Rotation = rotation;
    }

    /// <summary>
    ///     Gets the cell corresponding to this location.
    /// </summary>
    public Cell Cell
    {
        get => new(Cell.CellFromCoordinate(Position.X), Cell.CellFromCoordinate(Position.Z));
    }

    /// <summary>
    ///     Gets the position represented by this location.
    /// </summary>
    /// <value>The position.</value>
    public Vector3d Position { get; init; }

    /// <summary>
    ///     Gets the rotation represented by this location.
    /// </summary>
    /// <value>The rotation.</value>
    public Rotation Rotation { get; init; }

    /// <summary>
    ///     Gets the world represented by this location.
    /// </summary>
    /// <value>The world.</value>
    public World World { get; init; }

    /// <summary>
    ///     Determines if two <see cref="Location" /> instances are equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool operator ==(Location left, Location right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Determines if two <see cref="Location" /> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public static bool operator !=(Location left, Location right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    ///     Determines if two <see cref="Location" /> instances are equal.
    /// </summary>
    /// <param name="other">The other instance.</param>
    /// <returns>
    ///     <see langword="true" /> if this instance is equal to <paramref name="other" />; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(Location other)
    {
        return Position.Equals(other.Position) && Rotation.Equals(other.Rotation) && World.Equals(other.World);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Location other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Position, Rotation, World);
    }

    /// <summary>
    ///     Converts this <see cref="Location" /> to an instance of <see cref="Coordinates" />.
    /// </summary>
    /// <returns>The result of the conversion to <see cref="Coordinates" />.</returns>
    public Coordinates ToCoordinates()
    {
        (double x, double y, double z) = Position;
        return new Coordinates(World?.Name, x, y, z, Rotation.Yaw);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Location [World={World}, Position={Position}, Rotation={Rotation}]";
    }
}
