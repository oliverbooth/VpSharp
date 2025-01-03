﻿using System.Globalization;

namespace VpSharp;

/// <summary>
///     Represents a set of coordinates.
/// </summary>
public readonly partial struct Coordinates : IEquatable<Coordinates>, IFormattable
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Coordinates" /> struct.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="z">The Z coordinate.</param>
    /// <param name="yaw">The yaw.</param>
    /// <param name="isRelative">
    ///     <see langword="true" /> if these coordinates represent relative coordinates; <see langword="false" /> otherwise.
    /// </param>
    public Coordinates(double x, double y, double z, Heading yaw, bool isRelative = false)
        : this(null, x, y, z, yaw.ToHeading(), isRelative)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Coordinates" /> struct.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="z">The Z coordinate.</param>
    /// <param name="yaw">The yaw.</param>
    /// <param name="isRelative">
    ///     <see langword="true" /> if these coordinates represent relative coordinates; <see langword="false" /> otherwise.
    /// </param>
    public Coordinates(double x, double y, double z, double yaw, bool isRelative = false)
        : this(null, x, y, z, yaw, isRelative)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Coordinates" /> struct.
    /// </summary>
    /// <param name="world">The world name.</param>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="z">The Z coordinate.</param>
    /// <param name="yaw">The yaw.</param>
    /// <param name="isRelative">
    ///     <see langword="true" /> if these coordinates represent relative coordinates; <see langword="false" /> otherwise.
    /// </param>
    public Coordinates(string? world, double x, double y, double z, Heading yaw, bool isRelative = false)
        : this(world, x, y, z, yaw.ToHeading(), isRelative)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Coordinates" /> struct.
    /// </summary>
    /// <param name="world">The world name.</param>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="z">The Z coordinate.</param>
    /// <param name="yaw">The yaw.</param>
    /// <param name="isRelative">
    ///     <see langword="true" /> if these coordinates represent relative coordinates; <see langword="false" /> otherwise.
    /// </param>
    public Coordinates(string? world, double x, double y, double z, double yaw, bool isRelative = false)
    {
        World = world;
        X = x;
        Y = y;
        Z = z;
        Yaw = yaw % 360.0;
        IsRelative = isRelative;
    }

    /// <summary>
    ///     Gets or initializes the heading.
    /// </summary>
    /// <value>The heading.</value>
    public Heading Heading
    {
        get => Yaw.ToHeading();
        init => Yaw = value.ToHeading();
    }

    /// <summary>
    ///     Gets or initializes a value indicating whether this instance represents relative coordinates.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if this instance represents relative coordinates; otherwise, <see langword="false" />.
    /// </value>
    public bool IsRelative { get; init; }

    /// <summary>
    ///     Gets or initializes the position by accessing the <see cref="X" />, <see cref="Y" />, and <see cref="Z" /> properties.
    /// </summary>
    /// <value>The position encoded in these coordinates.</value>
    public Vector3d Position
    {
        get => new(X, Y, Z);
        init
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
        }
    }

    /// <summary>
    ///     Gets or initializes the world.
    /// </summary>
    /// <value>The world.</value>
    public string? World { get; init; }

    /// <summary>
    ///     Gets or initializes the X coordinate.
    /// </summary>
    /// <value>The X coordinate.</value>
    public double X { get; init; }

    /// <summary>
    ///     Gets or initializes the Y coordinate.
    /// </summary>
    /// <value>The Y coordinate.</value>
    public double Y { get; init; }

    /// <summary>
    ///     Gets or initializes the yaw.
    /// </summary>
    /// <value>The yaw.</value>
    public double Yaw { get; init; }

    /// <summary>
    ///     Gets or initializes the Z coordinate.
    /// </summary>
    /// <value>The Z coordinate.</value>
    public double Z { get; init; }

    /// <summary>
    ///     Returns a value indicating whether two instances of <see cref="Coordinates" /> are equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns><see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(in Coordinates left, in Coordinates right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Returns a value indicating whether two instances of <see cref="Coordinates" /> are not equal.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns><see langword="true" /> if the two instances are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(in Coordinates left, in Coordinates right)
    {
        return !(left == right);
    }

    /// <summary>
    ///     Parses a coordinate string.
    /// </summary>
    /// <param name="coordinates">The coordinates to parse.</param>
    /// <returns>An instance of <see cref="Coordinates" />.</returns>
    public static Coordinates Parse(string coordinates)
    {
        return Serializer.Deserialize(coordinates);
    }

    /// <summary>
    ///     Returns a value indicating whether this instance of <see cref="Coordinates" /> and another instance of
    ///     <see cref="Coordinates" /> are equal.
    /// </summary>
    /// <param name="other">The instance against which to compare.</param>
    /// <returns>
    ///     <see langword="true" /> if this instance is equal to <paramref name="other" />; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(Coordinates other)
    {
        return X.Equals(other.X) &&
               Y.Equals(other.Y) &&
               Z.Equals(other.Z) &&
               Yaw.Equals(other.Yaw) &&
               IsRelative.Equals(other.IsRelative) &&
               string.Equals(World, other.World, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Coordinates other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(World, X, Y, Z, Yaw);
    }

    /// <summary>
    ///     Returns the string representation of these coordinates.
    /// </summary>
    /// <returns>A <see cref="string" /> representation of these coordinates.</returns>
    public override string ToString()
    {
        return ToString("F", CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Returns the string representation of these coordinates.
    /// </summary>
    /// <param name="format">The format to apply to each component.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>A <see cref="string" /> representation of these coordinates.</returns>
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        format ??= "F";
        return Serializer.Serialize(this, format, formatProvider ?? CultureInfo.InvariantCulture);
    }
}
