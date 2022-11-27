using System.Drawing;
using VpSharp.Internal;

namespace VpSharp;

/// <summary>
///     Represents a color composed of single-precision floating point values. This structure is intended to behave similarly
///     to <see cref="Color" />, but uses <see cref="float" /> components rather than <see cref="byte" />.
/// </summary>
/// <seealso cref="Color" />
public readonly struct ColorF : IEquatable<ColorF>
{
    private ColorF(float a, float r, float g, float b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
    }

    /// <summary>
    ///     Gets or initializes the alpha component value of this <see cref="ColorF" /> structure.
    /// </summary>
    /// <value>The alpha component value of this <see cref="ColorF" />.</value>
    public float A { get; init; }

    /// <summary>
    ///     Gets or initializes the red component value of this <see cref="ColorF" /> structure.
    /// </summary>
    /// <value>The red component value of this <see cref="ColorF" />.</value>
    public float R { get; init; }

    /// <summary>
    ///     Gets or initializes the green component value of this <see cref="ColorF" /> structure.
    /// </summary>
    /// <value>The green component value of this <see cref="ColorF" />.</value>
    public float G { get; init; }

    /// <summary>
    ///     Gets or initializes the blue component value of this <see cref="ColorF" /> structure.
    /// </summary>
    /// <value>The blue component value of this <see cref="ColorF" />.</value>
    public float B { get; init; }

    /// <summary>
    ///     Returns a value indicating whether the two given colors are equal.
    /// </summary>
    /// <param name="left">The first color to compare.</param>
    /// <param name="right">The second color to compare.</param>
    /// <returns><see langword="true" /> if the two colors are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(ColorF left, ColorF right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Returns a value indicating whether the two given colors are equal.
    /// </summary>
    /// <param name="left">The first color to compare.</param>
    /// <param name="right">The second color to compare.</param>
    /// <returns><see langword="true" /> if the two colors are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(ColorF left, ColorF right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    ///     Implicitly converts an instance of <see cref="Color" /> to an instance of <see cref="ColorF" />.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>The converted color.</returns>
    public static implicit operator ColorF(Color color)
    {
        return FromArgb(color.A / 255.0f, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
    }

    /// <summary>
    ///     Implicitly converts an instance of <see cref="ColorF" /> to an instance of <see cref="Color" />.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>The converted color.</returns>
    public static explicit operator Color(ColorF color)
    {
        return Color.FromArgb((int) (color.A * 255.0f), (int) (color.R * 255.0f), (int) (color.G * 255.0f),
            (int) (color.B * 255.0f));
    }

    /// <summary>
    ///     Creates a <see cref="ColorF" /> structure from the specified single-precision floating-point color values (red,
    ///     green, and blue). The alpha value is implicitly 1.0 (fully opaque).
    /// </summary>
    /// <param name="r">The red component value for the new <see cref="ColorF" />. Valid values are 0.0 through 1.0.</param>
    /// <param name="g">The green component value for the new <see cref="ColorF" />. Valid values are 0.0 through 1.0.</param>
    /// <param name="b">The blue component value for the new <see cref="ColorF" />. Valid values are 0.0 through 1.0.</param>
    /// <returns>The <see cref="ColorF" /> that this method creates.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="r" />, <paramref name="g" />, or <paramref name="b" /> is less than 0 or greater than 1.
    /// </exception>
    public static ColorF FromArgb(float r, float g, float b)
    {
        return FromArgb(1.0f, r, g, b);
    }

    /// <summary>
    ///     Creates a <see cref="ColorF" /> structure from the specified single-precision floating-point color values (alpha,
    ///     red, green, and blue).
    /// </summary>
    /// <param name="a">The alpha component value for the new <see cref="ColorF" />. Valid values are 0.0 through 1.0.</param>
    /// <param name="r">The red component value for the new <see cref="ColorF" />. Valid values are 0.0 through 1.0.</param>
    /// <param name="g">The green component value for the new <see cref="ColorF" />. Valid values are 0.0 through 1.0.</param>
    /// <param name="b">The blue component value for the new <see cref="ColorF" />. Valid values are 0.0 through 1.0.</param>
    /// <returns>The <see cref="ColorF" /> that this method creates.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="a" />, <paramref name="r" />, <paramref name="g" />, or <paramref name="b" /> is less than 0 or
    ///     greater than 1.
    /// </exception>
    public static ColorF FromArgb(float a, float r, float g, float b)
    {
        if (a is < 0 or > 1) throw ThrowHelper.ZeroThroughOneException(nameof(a));
        if (r is < 0 or > 1) throw ThrowHelper.ZeroThroughOneException(nameof(r));
        if (g is < 0 or > 1) throw ThrowHelper.ZeroThroughOneException(nameof(g));
        if (b is < 0 or > 1) throw ThrowHelper.ZeroThroughOneException(nameof(b));

        return new ColorF(a, r, g, b);
    }

    /// <summary>
    ///     Returns a value indicating whether this color and another color are equal.
    /// </summary>
    /// <param name="other">The color to compare with this instance.</param>
    /// <returns><see langword="true" /> if the two colors are equal; otherwise, <see langword="false" />.</returns>
    public bool Equals(ColorF other)
    {
        return A.Equals(other.A) && R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ColorF other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(A, R, G, B);
    }
}