using System.Drawing;

namespace VpSharp.Extensions;

internal static class ColorExtensions
{
    /// <summary>
    ///     Deconstructs this color into RGB components.
    /// </summary>
    /// <param name="color">The color to deconstruct.</param>
    /// <param name="r">The red component.</param>
    /// <param name="g">The green component.</param>
    /// <param name="b">The blue component.</param>
    public static void Deconstruct(this Color color, out byte r, out byte g, out byte b)
    {
        r = color.R;
        g = color.G;
        b = color.B;
    }

    /// <summary>
    ///     Deconstructs this color into ARGB components.
    /// </summary>
    /// <param name="color">The color to deconstruct.</param>
    /// <param name="a">The alpha component.</param>
    /// <param name="r">The red component.</param>
    /// <param name="g">The green component.</param>
    /// <param name="b">The blue component.</param>
    public static void Deconstruct(this Color color, out byte a, out byte r, out byte g, out byte b)
    {
        (r, g, b) = color;
        a = color.A;
    }
}
