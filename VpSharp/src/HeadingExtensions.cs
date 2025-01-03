namespace VpSharp;

/// <summary>
///     Extension methods to convert to/from <see cref="Heading" /> and <see cref="float" />.
/// </summary>
public static class HeadingExtensions
{
    /// <summary>
    ///     Converts a <see cref="Heading" /> to an angle representing the heading.
    /// </summary>
    /// <param name="heading">The heading to convert.</param>
    /// <returns>A value from <see cref="Heading" />.</returns>
    public static Heading ToHeading(this double heading)
    {
        heading %= 360;
        if (heading < 0)
        {
            heading += 360;
        }

        return ((int)heading / 45) switch
        {
            1 => Heading.NE,
            2 => Heading.E,
            3 => Heading.SE,
            4 => Heading.S,
            5 => Heading.SW,
            6 => Heading.W,
            7 => Heading.NW,
            _ => Heading.N
        };
    }

    /// <summary>
    ///     Converts a <see cref="Heading" /> to an angle representing the heading.
    /// </summary>
    /// <param name="direction">The heading to convert.</param>
    /// <returns>The heading, in degrees.</returns>
    public static double ToHeading(this Heading direction)
    {
        return direction switch
        {
            Heading.N => 0,
            Heading.NE => 45,
            Heading.E => 90,
            Heading.SE => 135,
            Heading.S => 180,
            Heading.SW => 225,
            Heading.W => 270,
            Heading.NW => 315,
            _ => 0
        };
    }
}
