using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace VpSharp;

/// <summary>
///     Represents a vector with three double-precision floating-point values. This structure very closely resembles the API
///     provided by <see cref="Vector3" />, however its components and the mathematical operations performed on them use
///     <see cref="double" /> arithmetic. 
/// </summary>
public struct Vector3d : IEquatable<Vector3d>, IFormattable
{
    /// <summary>
    ///     A vector whose three elements are equal to one (that is, the vector <c>(1, 1, 1)</c>).
    /// </summary>
    public static readonly Vector3d One = new(1, 1, 1);

    /// <summary>
    ///     The vector <c>(1, 0, 0)</c>.
    /// </summary>
    public static readonly Vector3d UnitX = new(1, 0, 0);

    /// <summary>
    ///     The vector <c>(0, 1, 0)</c>.
    /// </summary>
    public static readonly Vector3d UnitY = new(0, 1, 0);

    /// <summary>
    ///     The vector <c>(0, 0, 1)</c>.
    /// </summary>
    public static readonly Vector3d UnitZ = new(0, 0, 1);

    /// <summary>
    ///     A vector whose three elements are equal to zero (that is, the vector <c>(0, 0, 0)</c>).
    /// </summary>
    public static readonly Vector3d Zero = new(0, 0, 0);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vector3d" /> struct, whose three elements have the same value.
    /// </summary>
    public Vector3d(double value) : this(value, value, value)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vector3d" /> struct, whose elements have the specified values.
    /// </summary>
    /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
    /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
    /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
    public Vector3d(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vector3d" /> struct from the given
    ///     <see cref="ReadOnlySpan{Single}" />. The span must contain at least 3 elements.
    /// </summary>
    /// <param name="values">The span of elements to assign to the vector.</param>
    /// <exception cref="IndexOutOfRangeException"><paramref name="values" /> contains fewer than 3 elements.</exception>
    public Vector3d(ReadOnlySpan<double> values)
    {
        if (values.Length < 3)
            throw new IndexOutOfRangeException("The specified span has an insufficient number of elements.");

        this = Unsafe.ReadUnaligned<Vector3d>(ref Unsafe.As<double, byte>(ref MemoryMarshal.GetReference(values)));
    }

    /// <summary>
    ///     Gets the length of the vector.
    /// </summary>
    /// <value>The length of the vector.</value>
    /// <seealso cref="LengthSquared" />
    public double Length => Distance(this, Zero);

    /// <summary>
    ///     Gets the squared length of the vector.
    /// </summary>
    /// <value>The length of the vector, squared.</value>
    /// <seealso cref="Length" />
    public double LengthSquared => DistanceSquared(this, Zero);

    /// <summary>
    ///     Gets or sets the X component of the vector.
    /// </summary>
    /// <value>The X component of the vector.</value>
    public double X { get; set; }

    /// <summary>
    ///     Gets or sets the Y component of the vector.
    /// </summary>
    /// <value>The Y component of the vector.</value>
    public double Y { get; set; }

    /// <summary>
    ///     Gets or sets The Z component of the vector.
    /// </summary>
    /// <value>The Z component of the vector.</value>
    public double Z { get; set; }

    /// <summary>
    ///     Adds two vectors together.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>The summed vector.</returns>
    public static Vector3d operator +(Vector3d left, Vector3d right)
    {
        return new(
            left.X + right.X,
            left.Y + right.Y,
            left.Z + right.Z
        );
    }

    /// <summary>
    ///     Subtracts the second vector from the first.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>The difference vector.</returns>
    public static Vector3d operator -(Vector3d left, Vector3d right)
    {
        return new(
            left.X - right.X,
            left.Y - right.Y,
            left.Z - right.Z
        );
    }

    /// <summary>
    ///     Multiples two vectors together.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>The product vector.</returns>
    public static Vector3d operator *(Vector3d left, Vector3d right)
    {
        return new(
            left.X * right.X,
            left.Y * right.Y,
            left.Z * right.Z
        );
    }

    /// <summary>
    ///     Multiples a vector by the given scalar.
    /// </summary>
    /// <param name="left">The vector value.</param>
    /// <param name="right">The scalar value.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector3d operator *(Vector3d left, double right)
    {
        return new(
            left.X * right,
            left.Y * right,
            left.Z * right
        );
    }

    /// <summary>
    ///     Multiples a vector by the given scalar.
    /// </summary>
    /// <param name="left">The scalar value.</param>
    /// <param name="right">The vector value.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector3d operator *(double left, Vector3d right)
    {
        return new(
            left * right.X,
            left * right.Y,
            left * right.Z
        );
    }

    /// <summary>
    ///     Divides the first vector by the second.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>The vector resulting from the division.</returns>
    public static Vector3d operator /(Vector3d left, Vector3d right)
    {
        return new(
            left.X / right.X,
            left.Y / right.Y,
            left.Z / right.Z
        );
    }

    /// <summary>
    ///     Divides the vector by the given scalar.
    /// </summary>
    /// <param name="left">The vector value.</param>
    /// <param name="right">The scalar value.</param>
    /// <returns>The vector resulting from the division.</returns>
    public static Vector3d operator /(Vector3d left, double right)
    {
        return new(
            left.X / right,
            left.Y / right,
            left.Z / right
        );
    }

    /// <summary>
    ///     Negates a given vector.
    /// </summary>
    /// <param name="value">The source vector.</param>
    /// <returns>The negated vector.</returns>
    public static Vector3d operator -(Vector3d value)
    {
        return Zero - value;
    }

    /// <summary>
    ///     Returns a value indicating whether the two given vectors are equal.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><see langword="true" /> if the two vectors are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Vector3d left, Vector3d right) => left.Equals(right);

    /// <summary>
    ///     Returns a value indicating whether the two given vectors are not equal.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><see langword="true" /> if the two vectors are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Vector3d left, Vector3d right) => !left.Equals(right);

    /// <summary>
    ///     Implicitly converts a <see cref="Vector3" /> to a new instance of <see cref="Vector3d" />, by implicitly
    ///     converting the <see cref="Vector3.X" />, <see cref="Vector3.Y" /> and <see cref="Vector3.Z" /> fields to
    ///     <see cref="double" />.
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>The converted vector.</returns>
    public static implicit operator Vector3d(Vector3 vector) => new(vector.X, vector.Y, vector.Z);

    /// <summary>
    ///     Explicit converts a <see cref="Vector3" /> to a new instance of <see cref="Vector3d" />, by explicitly
    ///     converting the <see cref="Vector3.X" />, <see cref="Vector3.Y" /> and <see cref="Vector3.Z" /> fields to
    ///     <see cref="float" />.
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>The converted vector.</returns>
    public static explicit operator Vector3(Vector3d vector) => new((float) vector.X, (float) vector.Y, (float) vector.Z);

    /// <summary>
    ///     Returns a vector whose elements are the absolute values of each of the source vector's elements.
    /// </summary>
    /// <param name="value">The source vector.</param>
    /// <returns>The absolute value vector.</returns>
    public static Vector3d Abs(Vector3d value)
    {
        return new(
            Math.Abs(value.X),
            Math.Abs(value.Y),
            Math.Abs(value.Z)
        );
    }

    /// <summary>
    ///     Restricts a vector between a minimum and maximum value.
    /// </summary>
    /// <param name="value">The source vector.</param>
    /// <param name="min">The minimum vector</param>
    /// <param name="max">The maximum vector</param>
    /// <returns>The restricted vector.</returns>
    public static Vector3d Clamp(Vector3d value, Vector3d min, Vector3d max)
    {
        return Min(Max(value, min), max);
    }

    /// <summary>
    ///     Computes the cross product of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The cross product.</returns>
    public static Vector3d Cross(Vector3d left, Vector3d right)
    {
        return new(
            left.Y * right.Z - left.Z * right.Y,
            left.Z * right.X - left.X * right.Z,
            left.X * right.Y - left.Y * right.X
        );
    }

    /// <summary>
    ///     Returns the Euclidean distance between two points.
    /// </summary>
    /// <param name="left">The first point.</param>
    /// <param name="right">The second point.</param>
    /// <returns>The distance.</returns>
    public static double Distance(Vector3d left, Vector3d right)
    {
        Vector3d difference = left - right;
        double dot = Dot(difference, difference);
        return Math.Sqrt(dot);
    }

    /// <summary>
    ///     Returns the squared Euclidean distance between two points.
    /// </summary>
    /// <param name="left">The first point.</param>
    /// <param name="right">The second point.</param>
    /// <returns>The distance squared.</returns>
    public static double DistanceSquared(Vector3d left, Vector3d right)
    {
        Vector3d difference = left - right;
        return Dot(difference, difference);
    }

    /// <summary>
    ///     Returns the dot product of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The dot product.</returns>
    public static double Dot(Vector3d left, Vector3d right)
    {
        return left.X * right.X +
               left.Y * right.Y +
               left.Z * right.Z;
    }

    /// <summary>
    ///     Linearly interpolates between two vectors based on the given weighting.
    /// </summary>
    /// <param name="a">The first source vector.</param>
    /// <param name="b">The second source vector.</param>
    /// <param name="t">A value between 0 and 1 indicating the weight of <paramref name="b" />.</param>
    /// <returns>The interpolate vector.</returns>
    public static Vector3d Lerp(Vector3d a, Vector3d b, double t)
    {
        Vector3d firstInfluence = a * (1.0f - t);
        Vector3d secondInfluence = b * t;
        return firstInfluence + secondInfluence;
    }

    /// <summary>
    ///     Returns a vector whose elements are the maximum of each of the pairs of elements in the two source vectors.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>The maximized vector.</returns>
    public static Vector3d Max(Vector3d left, Vector3d right)
    {
        return new(
            Math.Max(left.X, right.X),
            Math.Max(left.Y, right.Y),
            Math.Max(left.Z, right.Z)
        );
    }

    /// <summary>
    ///     Returns a vector whose elements are the minimum of each of the pairs of elements in the two source vectors.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>The minimized vector.</returns>
    public static Vector3d Min(Vector3d left, Vector3d right)
    {
        return new(
            Math.Min(left.X, right.X),
            Math.Min(left.Y, right.Y),
            Math.Min(left.Z, right.Z)
        );
    }

    /// <summary>
    ///     Returns a vector with the same direction as the given vector, but with a length of 1.
    /// </summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    public static Vector3d Normalize(Vector3d value)
    {
        return value / value.Length;
    }

    /// <summary>
    ///     Returns the reflection of a vector off a surface that has the specified normal.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="normal">The normal of the surface off of which the vector is being reflected.</param>
    /// <returns>The reflected vector.</returns>
    public static Vector3d Reflect(Vector3d vector, Vector3d normal)
    {
        double dot = Dot(vector, normal);
        Vector3d temp = normal * dot * 2.0;
        return vector - temp;
    }

    /// <summary>
    ///     Returns a vector whose elements are the square root of each of the source vector's elements.
    /// </summary>
    /// <param name="value">The source vector.</param>
    /// <returns>The square root vector.</returns>
    public static Vector3d SquareRoot(Vector3d value)
    {
        return new(
            Math.Sqrt(value.X),
            Math.Sqrt(value.Y),
            Math.Sqrt(value.Z)
        );
    }

    /// <summary>
    ///     Copies the contents of the vector into the given array.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="index">The starting index in the array to which the values should be written.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="index" /> is outside of the bounds of the array.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     The number of elements in the vector is greater than the size of <paramref name="array" />.
    /// </exception>
    public readonly void CopyTo(double[] array, int index = 0)
    {
        if (array is null)
            throw new ArgumentNullException(nameof(array));

        if (index < 0 || index >= array.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Specified index was out of the bounds of the array.");

        if (array.Length - index < 3)
            throw new ArgumentException("The number of elements in source vector is greater than the destination array.");

        array[index] = X;
        array[index + 1] = Y;
        array[index + 2] = Z;
    }

    /// <summary>
    ///     Deconstructs this vector.
    /// </summary>
    /// <param name="x">The X component value.</param>
    /// <param name="y">The Y component value.</param>
    /// <param name="z">The Z component value.</param>
    public readonly void Deconstruct(out double x, out double y, out double z)
    {
        x = X;
        y = Y;
        z = Z;
    }

    /// <summary>
    ///     Returns a value indicating whether this vector and another vector are equal.
    /// </summary>
    /// <param name="other">The vector to compare with this instance.</param>
    /// <returns><see langword="true" /> if the two vectors are equal; otherwise, <see langword="false" />.</returns>
    public readonly bool Equals(Vector3d other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

    /// <inheritdoc />
    public override readonly bool Equals(object? obj) => obj is Vector3d other && Equals(other);

    /// <inheritdoc />
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);

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
    public readonly string ToString(string format)
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
        builder.Append(Y.ToString(format, formatProvider));
        builder.Append(separator);
        builder.Append(' ');
        builder.Append(Z.ToString(format, formatProvider));
        builder.Append('>');
        return builder.ToString();
    }
}
