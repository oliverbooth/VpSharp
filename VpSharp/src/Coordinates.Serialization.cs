using System.Globalization;
using System.Text;
using Cysharp.Text;
using X10D.Linq;
using X10D.Text;

namespace VpSharp;

public readonly partial struct Coordinates
{
    private static class Serializer
    {
        public static string Serialize(in Coordinates coordinates, string format)
        {
            int count = Serialize(coordinates, format, Span<char>.Empty);
            Span<char> chars = stackalloc char[count];
            Serialize(coordinates, format, chars);
            return chars.ToString();
        }

        public static int Serialize(in Coordinates coordinates, string format, Span<char> destination)
        {
            using Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();

            if (!string.IsNullOrWhiteSpace(coordinates.World))
            {
                builder.Append(coordinates.World);
                builder.Append(' ');
            }

            bool north = coordinates.Z >= 0.0;
            bool west = coordinates.X >= 0.0;
            bool up = coordinates.Y >= 0.0;
            bool dir = coordinates.Yaw >= 0.0;

            if (coordinates.IsRelative)
            {
                if (north)
                {
                    builder.Append('+');
                }

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.Z));
                builder.Append(' ');

                if (west)
                {
                    builder.Append('+');
                }

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.X));
                builder.Append(' ');

                if (up)
                {
                    builder.Append('+');
                }

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.Y));
                builder.Append("a ");

                if (dir)
                {
                    builder.Append('+');
                }

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.Yaw));
            }
            else
            {
                char zChar = north ? 'n' : 's';
                char xChar = west ? 'w' : 'e';

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, Math.Abs(coordinates.Z)));
                builder.Append(zChar);
                builder.Append(' ');

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, Math.Abs(coordinates.X)));
                builder.Append(xChar);
                builder.Append(' ');

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.Y));
                builder.Append("a ");

                builder.Append(string.Format(CultureInfo.InvariantCulture, format, coordinates.Yaw));
            }

            ReadOnlySpan<byte> bytes = builder.AsSpan();
            Span<char> chars = stackalloc char[bytes.Length];
            Encoding.UTF8.GetChars(bytes, chars);

            for (var index = 0; index < destination.Length; index++)
            {
                destination[index] = chars[index];
            }

            return builder.Length;
        }

        public static Coordinates Deserialize(ReadOnlySpan<char> value)
        {
            Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();
            string? world = null;
            var isRelative = false;
            double x = 0.0, y = 0.0, z = 0.0, yaw = 0.0;

            var word = 0;
            for (var index = 0; index < value.Length; index++)
            {
                char current = value[index];
                bool atEnd = index == value.Length - 1;

                if (atEnd || char.IsWhiteSpace(current))
                {
                    if (!builder.AsSpan().All(b => char.IsWhiteSpace((char)b)))
                    {
                        if (atEnd)
                        {
                            builder.Append(current);
                        }

                        ProcessBuffer(ref builder, ref world, word, ref isRelative, ref z, ref x, ref y, ref yaw);
                        word++;
                    }

                    builder.Clear();
                }
                else
                {
                    builder.Append(current);
                }
            }

            builder.Dispose();
            return new Coordinates(world, x, y, z, yaw, isRelative);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a relative unit string.
        /// </summary>
        /// <param name="bytes">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="bytes" /> represents a valid relative unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsAbsoluteUnit(ReadOnlySpan<byte> bytes)
        {
            Span<char> chars = stackalloc char[bytes.Length];
            Encoding.UTF8.GetChars(bytes, chars);
            return IsRelativeUnit(chars);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a relative unit string.
        /// </summary>
        /// <param name="chars">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="chars" /> represents a valid relative unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsAbsoluteUnit(ReadOnlySpan<char> chars)
        {
            ReadOnlySpan<char> validChars = "nNeEwWsSaA";
            return double.TryParse(chars, out _) ||
                   (validChars.Contains(chars[^1]) &&
                    double.TryParse(chars[..^1], NumberStyles.Float, CultureInfo.InvariantCulture, out _));
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a relative unit string.
        /// </summary>
        /// <param name="bytes">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="bytes" /> represents a valid relative unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsRelativeUnit(ReadOnlySpan<byte> bytes)
        {
            Span<char> chars = stackalloc char[bytes.Length];
            Encoding.UTF8.GetChars(bytes, chars);
            return IsRelativeUnit(chars);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a relative unit string.
        /// </summary>
        /// <param name="chars">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="chars" /> represents a valid relative unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsRelativeUnit(ReadOnlySpan<char> chars)
        {
            return (chars[0] == '+' || chars[0] == '-') &&
                   double.TryParse(chars, NumberStyles.Float, CultureInfo.InvariantCulture, out _);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a valid coordinate unit string.
        /// </summary>
        /// <param name="bytes">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="bytes" /> represents a valid coordinate unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsUnitString(ReadOnlySpan<byte> bytes)
        {
            Span<char> chars = stackalloc char[bytes.Length];
            Encoding.UTF8.GetChars(bytes, chars);
            return IsUnitString(chars);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified span of characters represents a valid coordinate unit string.
        /// </summary>
        /// <param name="chars">The span of characters to validate.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="chars" /> represents a valid coordinate unit string; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public static bool IsUnitString(ReadOnlySpan<char> chars)
        {
            chars = chars.Trim();

            if (chars.Length == 0)
            {
                return false;
            }

            if (!char.IsDigit(chars[0]) && chars[0] != '+' && chars[0] != '-')
            {
                return false;
            }

            //                              thicc char span
            return IsRelativeUnit(chars) || IsAbsoluteUnit(chars);
        }


        private static bool IsAtWordIndex(int expected, int actual, bool hasWorld)
        {
            return hasWorld ? actual == expected + 1 : actual == expected;
        }

        private static void ProcessBuffer(
            ref Utf8ValueStringBuilder builder,
            ref string? world,
            int word,
            ref bool isRelative,
            ref double z,
            ref double x,
            ref double y,
            ref double yaw
        )
        {
            ReadOnlySpan<byte> bytes = builder.AsSpan();
            Span<char> chars = stackalloc char[bytes.Length];
            Encoding.UTF8.GetChars(bytes, chars);
            bool hasWorld = !string.IsNullOrWhiteSpace(world);

            if (word == 0 && !IsUnitString(bytes))
            {
                world = chars.ToString().AsNullIfWhiteSpace();
            }
            else if (IsRelativeUnit(bytes))
            {
                isRelative = true;
                ProcessRelativeUnit(word, ref z, ref x, ref y, ref yaw, hasWorld, chars);
            }
            else
            {
                ProcessAbsoluteUnit(word, ref z, ref x, ref y, ref yaw, hasWorld, chars);
            }
        }

        private static void ProcessAbsoluteUnit(int word, ref double z, ref double x, ref double y, ref double yaw, bool hasWorld,
            Span<char> chars)
        {
            const StringComparison comparison = StringComparison.OrdinalIgnoreCase;
            char lastChar = chars[^1];

            bool isAt0 = IsAtWordIndex(0, word, hasWorld);
            bool isAt1 = IsAtWordIndex(1, word, hasWorld);
            bool isAt2 = IsAtWordIndex(2, word, hasWorld);
            bool isAt3 = IsAtWordIndex(3, word, hasWorld);

            Span<char> charsExceptLast = chars[..^1];
            double.TryParse(charsExceptLast, NumberStyles.Float, CultureInfo.InvariantCulture, out double value);

            if (isAt0)
            {
                if ("nzNZ".AsSpan().Contains(lastChar))
                {
                    z = value;
                }
                else if ("sS".AsSpan().Contains(lastChar))
                {
                    z = -value;
                }
            }
            else if (isAt1)
            {
                if ("xwXW".AsSpan().Contains(lastChar))
                {
                    x = value;
                }
                else if ("eE".AsSpan().Contains(lastChar))
                {
                    x = -value;
                }
            }
            else if (isAt2 && "aA".AsSpan().Contains(lastChar))
            {
                y = value;
            }
            else if (isAt3 && double.TryParse(chars, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                yaw = value;
            }

            /*
            if (isAt1 && "XW".Contains(lastChar, comparison))
            {
                x = value;
            }
            else if (isAt0 && "ZN".Contains(lastChar, comparison))
            {
                z = value;
            }
            else if (isAt1 && "E".Contains(lastChar, comparison))
            {
                x = -value;
            }
            else if (isAt0 && "S".Contains(lastChar, comparison))
            {
                z = -value;
            }
            else if (isAt2 && "A".Contains(lastChar, comparison))
            {
                y = value;
            }
            else if (isAt3 && double.TryParse(chars, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                yaw = value;
            }*/
        }

        private static void ProcessRelativeUnit(
            int word,
            ref double z,
            ref double x,
            ref double y,
            ref double yaw,
            bool hasWorld,
            Span<char> chars
        )
        {
            if (!double.TryParse(chars, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return;
            }

            if (IsAtWordIndex(0, word, hasWorld))
            {
                z = value;
            }
            else if (IsAtWordIndex(1, word, hasWorld))
            {
                x = value;
            }
            else if (IsAtWordIndex(2, word, hasWorld))
            {
                y = value;
            }
            else if (IsAtWordIndex(3, word, hasWorld))
            {
                yaw = value;
            }
        }
    }
}
