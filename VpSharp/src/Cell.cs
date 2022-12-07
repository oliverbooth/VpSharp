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
    public static bool operator ==(Cell left, Cell right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Returns a value indicating whether the two given cells are equal.
    /// </summary>
    /// <param name="left">The first cell to compare.</param>
    /// <param name="right">The second cell to compare.</param>
    /// <returns><see langword="true" /> if the two cells are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Cell left, Cell right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    ///     Explicitly converts an instance of <see cref="Vector2" /> to an instance of <see cref="Cell" />.
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>
    ///     A cell whose <see cref="X" /> component is equal to <see cref="Vector2.X" />, and whose <see cref="Z" /> component
    ///     is equal to <see cref="Vector2.Y" />.
    /// </returns>
    public static explicit operator Cell(Vector2 vector)
    {
        return FromVector2(vector);
    }

    /// <summary>
    ///     Implicitly converts an instance of <see cref="Cell" /> to an instance of <see cref="Vector2" />.
    /// </summary>
    /// <param name="cell">The cell to convert.</param>
    /// <returns>
    ///     A vector whose <see cref="X" /> component is equal to <see cref="Cell.X" />, and whose <see cref="Vector2.Y" />
    ///     component is equal to <see cref="Z" />.
    /// </returns>
    public static implicit operator Vector2(Cell cell)
    {
        return cell.ToVector2();
    }

    /// <summary>
    ///     Explicitly converts an instance of <see cref="Vector3" /> to an instance of <see cref="Cell" />.
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>
    ///     A cell whose <see cref="X" /> component is equal to <see cref="Vector3.X" />, and whose <see cref="Z" /> component
    ///     is equal to <see cref="Vector3.Z" />.
    /// </returns>
    public static explicit operator Cell(Vector3 vector)
    {
        return FromVector3(vector);
    }

    /// <summary>
    ///     Implicitly converts an instance of <see cref="Cell" /> to an instance of <see cref="Vector3" />.
    /// </summary>
    /// <param name="cell">The cell to convert.</param>
    /// <returns>
    ///     A vector whose <see cref="X" /> component is equal to <see cref="Cell.X" />, and whose <see cref="Vector3.Z" />
    ///     component is equal to <see cref="Z" />, and whose <see cref="Vector3.Y" /> component is 0.
    /// </returns>
    public static implicit operator Vector3(Cell cell)
    {
        return cell.ToVector3();
    }

    /// <summary>
    ///     Explicitly converts an instance of <see cref="Vector3d" /> to an instance of <see cref="Cell" />.
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>
    ///     A cell whose <see cref="X" /> component is equal to <see cref="Vector3d.X" />, and whose <see cref="Z" />
    ///     component is equal to <see cref="Vector3d.Z" />.
    /// </returns>
    public static explicit operator Cell(Vector3d vector)
    {
        return FromVector3d(vector);
    }

    /// <summary>
    ///     Calculates the cell number for a specified coordinate value.
    /// </summary>
    /// <param name="value">The coordinate value.</param>
    /// <returns>The cell number.</returns>
    /// <remarks>
    ///     The way VP calculates cell number isn't a <c>floor(n)</c> as one might have thought, but instead
    ///     <c>n &lt; 0 ? (int)value - 1 : (int)value</c>. This helper method exists because of this idiosyncrasy of the world
    ///     server. 
    /// </remarks>
    public static int CellFromCoordinate(double value)
    {
        return value < 0 ? (int)value - 1 : (int)value;
    }

    /// <summary>
    ///     Converts an instance of <see cref="Vector2" /> to a new instance of <see cref="Cell" />.
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>The cell result of the conversion.</returns>
    public static Cell FromVector2(in Vector2 vector)
    {
        return new Cell((int)vector.X, (int)vector.Y);
    }

    /// <summary>
    ///     Converts an instance of <see cref="Vector3" /> to a new instance of <see cref="Cell" />.
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>The cell result of the conversion.</returns>
    public static Cell FromVector3(in Vector3 vector)
    {
        return new Cell((int)vector.X, (int)vector.Y);
    }

    /// <summary>
    ///     Converts an instance of <see cref="Vector3d" /> to a new instance of <see cref="Cell" />.
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>The cell result of the conversion.</returns>
    public static Cell FromVector3d(in Vector3d vector)
    {
        return new Cell((int)vector.X, (int)vector.Y);
    }

    /// <summary>
    ///     Returns a value indicating whether this cell and another cell are equal.
    /// </summary>
    /// <param name="other">The cell to compare with this instance.</param>
    /// <returns><see langword="true" /> if the two cells are equal; otherwise, <see langword="false" />.</returns>
    public bool Equals(Cell other)
    {
        return X == other.X && Z == other.Z;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Cell other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Z);
    }

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
    public string ToString(string? format)
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
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        StringBuilder builder = new StringBuilder();
        string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
        builder.Append('<');
        builder.Append(X.ToString(format, formatProvider));
        builder.Append(separator);
        builder.Append(' ');
        builder.Append(Z.ToString(format, formatProvider));
        builder.Append('>');
        return builder.ToString();
    }

    /// <summary>
    ///     Converts this cell to a <see cref="Vector2" />.
    /// </summary>
    /// <returns>The vector result of the conversion.</returns>
    public Vector2 ToVector2()
    {
        return new Vector2(X, Z);
    }

    /// <summary>
    ///     Converts this cell to a <see cref="Vector2" />.
    /// </summary>
    /// <returns>The vector result of the conversion.</returns>
    public Vector3 ToVector3()
    {
        return new Vector3(X, 0, Z);
    }

    /// <summary>
    ///     Converts this cell to a <see cref="Vector2" />.
    /// </summary>
    /// <returns>The vector result of the conversion.</returns>
    public Vector3d ToVector3d()
    {
        return new Vector3d(X, 0, Z);
    }
}
