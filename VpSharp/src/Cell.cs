using System.Globalization;
using System.Numerics;
using System.Text;

namespace VpSharp;

/// <summary>
///     Represents a cell.
/// </summary>
public readonly struct Cell : IEquatable<Cell>, IFormattable
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Cell" /> struct.
    /// </summary>
    /// <param name="x">The X coordinate of the cell.</param>
    /// <param name="z">The Z coordinate of the cell.</param>
    public Cell(int x, int z)
    {
        X = x;
        Z = z;
    }

    /// <summary>
    ///     Gets the X coordinate of this cell.
    /// </summary>
    /// <value>The X coordinate.</value>
    public int X { get; }

    /// <summary>
    ///     Gets the Z coordinate of this cell.
    /// </summary>
    /// <value>The Z coordinate.</value>
    public int Z { get; }

    /// <summary>
    ///     Returns a value indicating whether the two given cells are equal.
    /// </summary>
    /// <param name="left">The first cell to compare.</param>
    /// <param name="right">The second cell to compare.</param>
    /// <returns><see langword="true" /> if the two cells are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Cell left, Cell right) => left.Equals(right);

    /// <summary>
    ///     Returns a value indicating whether the two given cells are equal.
    /// </summary>
    /// <param name="left">The first cell to compare.</param>
    /// <param name="right">The second cell to compare.</param>
    /// <returns><see langword="true" /> if the two cells are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Cell left, Cell right) => !left.Equals(right);

    /// <summary>
    ///     Explicitly converts an instance of <see cref="Vector2" /> to an instance of <see cref="Cell" />. 
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>
    ///     A cell whose <see cref="X" /> component is equal to <see cref="Vector2.X" />, and whose <see cref="Z" /> component
    ///     is equal to <see cref="Vector2.Y" />.
    /// </returns>
    public static explicit operator Cell(Vector2 vector) => new((int)vector.X, (int)vector.Y);

    /// <summary>
    ///     Implicitly converts an instance of <see cref="Cell" /> to an instance of <see cref="Vector2" />. 
    /// </summary>
    /// <param name="cell">The cell to convert.</param>
    /// <returns>
    ///     A vector whose <see cref="X" /> component is equal to <see cref="Cell.X" />, and whose <see cref="Vector2.Y" />
    ///     component is equal to <see cref="Z" />.
    /// </returns>
    public static implicit operator Vector2(Cell cell) => new(cell.X, cell.Z);

    /// <summary>
    ///     Explicitly converts an instance of <see cref="Vector3" /> to an instance of <see cref="Cell" />. 
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>
    ///     A cell whose <see cref="X" /> component is equal to <see cref="Vector3.X" />, and whose <see cref="Z" /> component
    ///     is equal to <see cref="Vector3.Z" />.
    /// </returns>
    public static explicit operator Cell(Vector3 vector) => new((int)vector.X, (int)vector.Z);

    /// <summary>
    ///     Implicitly converts an instance of <see cref="Cell" /> to an instance of <see cref="Vector3" />. 
    /// </summary>
    /// <param name="cell">The cell to convert.</param>
    /// <returns>
    ///     A vector whose <see cref="X" /> component is equal to <see cref="Cell.X" />, and whose <see cref="Vector3.Z" />
    ///     component is equal to <see cref="Z" />, and whose <see cref="Vector3.Y" /> component is 0.
    /// </returns>
    public static implicit operator Vector3(Cell cell) => new(cell.X, 0, cell.Z);

    /// <summary>
    ///     Explicitly converts an instance of <see cref="Vector3d" /> to an instance of <see cref="Cell" />. 
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>
    ///     A cell whose <see cref="X" /> component is equal to <see cref="Vector3d.X" />, and whose <see cref="Z" />
    ///     component is equal to <see cref="Vector3d.Z" />.
    /// </returns>
    public static explicit operator Cell(Vector3d vector) => new((int)vector.X, (int)vector.Z);

    /// <summary>
    ///     Returns a value indicating whether this cell and another cell are equal.
    /// </summary>
    /// <param name="other">The cell to compare with this instance.</param>
    /// <returns><see langword="true" /> if the two cells are equal; otherwise, <see langword="false" />.</returns>
    public bool Equals(Cell other) => X == other.X && Z == other.Z;

    /// <inheritdoc />
    public override bool Equals(object obj) => obj is Cell other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Z);

    /// <summary>
    ///     Returns a <see cref="string" /> representing this <see cref="Vector3d" /> instance.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    /// <summary>
    ///     Returns a <see cref="string" /> representing this <see cref="Vector3d" /> instance, using the specified format to
    ///     format individual elements.
    /// </summary>
    /// <param name="format">The format of individual elements.</param>
    /// <returns>The string representation.</returns>
    public readonly string ToString(string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    /// <summary>
    ///     Returns a <see cref="string" /> representing this <see cref="Vector3d" /> instance, using the specified format to
    ///     format individual elements and the given <see cref="IFormatProvider" />.
    /// </summary>
    /// <param name="format">The format of individual elements.</param>
    /// <param name="formatProvider">The format provider to use when formatting elements.</param>
    /// <returns>The string representation.</returns>
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        var builder = new StringBuilder();
        string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
        builder.Append('<');
        builder.Append(X.ToString(format, formatProvider));
        builder.Append(separator);
        builder.Append(' ');
        builder.Append(Z.ToString(format, formatProvider));
        builder.Append('>');
        return builder.ToString();
    }
}