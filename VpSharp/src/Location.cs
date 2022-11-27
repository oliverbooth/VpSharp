using System.Numerics;
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
    public static readonly Location Nowhere = new(VirtualParadiseWorld.Nowhere);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Location" /> struct.
    /// </summary>
    /// <param name="world">The world.</param>
    /// <param name="position">The position.</param>
    /// <param name="rotation">The rotation.</param>
    /// <exception cref="ArgumentNullException"><paramref name="world" /> is <see langword="null" />.</exception>
    public Location(VirtualParadiseWorld world, Vector3d position = default, Quaternion rotation = default)
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
        get
        {
            var x = (int) Math.Floor(Position.X);
            var z = (int) Math.Floor(Position.Z);
            return new Cell(x, z);
        }
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
    public Quaternion Rotation { get; init; }

    /// <summary>
    ///     Gets the world represented by this location.
    /// </summary>
    /// <value>The world.</value>
    public VirtualParadiseWorld World { get; init; }

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

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Location [World={World}, Position={Position}, Rotation={Rotation}]";
    }
}
