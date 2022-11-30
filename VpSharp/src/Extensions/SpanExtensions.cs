using System.Globalization;
using System.Numerics;
using System.Text;
using Cysharp.Text;

namespace VpSharp.Extensions;

internal static class SpanExtensions
{
    public static double ToDouble(this ReadOnlySpan<byte> value)
    {
        Span<char> chars = stackalloc char[value.Length];
        Encoding.UTF8.GetChars(value, chars);
        return double.Parse(chars.Trim(), provider: CultureInfo.InvariantCulture);
    }

    public static int ToInt32(this ReadOnlySpan<byte> value)
    {
        Span<char> chars = stackalloc char[value.Length];
        Encoding.UTF8.GetChars(value, chars);
        return int.Parse(chars.Trim(), provider: CultureInfo.InvariantCulture);
    }

    public static float ToSingle(this ReadOnlySpan<byte> value)
    {
        Span<char> chars = stackalloc char[value.Length];
        Encoding.UTF8.GetChars(value, chars);
        return float.Parse(chars.Trim(), provider: CultureInfo.InvariantCulture);
    }

    public static Vector2 ToVector2(this ReadOnlySpan<byte> value)
    {
        Span<char> chars = stackalloc char[value.Length];
        Encoding.UTF8.GetChars(value, chars);
        return ToVector2(chars.Trim());
    }

    public static Vector3 ToVector3(this ReadOnlySpan<byte> value)
    {
        Span<char> chars = stackalloc char[value.Length];
        Encoding.UTF8.GetChars(value, chars);
        return ToVector3(chars.Trim());
    }

    public static Vector3d ToVector3d(this ReadOnlySpan<byte> value)
    {
        Span<char> chars = stackalloc char[value.Length];
        Encoding.UTF8.GetChars(value, chars);
        return ToVector3d(chars.Trim());
    }

    public static Vector4 ToVector4(this ReadOnlySpan<byte> value)
    {
        Span<char> chars = stackalloc char[value.Length];
        Encoding.UTF8.GetChars(value, chars);
        return ToVector4(chars.Trim());
    }

    public static Vector2 ToVector2(this ReadOnlySpan<char> value)
    {
        float x = 0;
        float y = 0;
        byte spaceCount = 0;

        using Utf8ValueStringBuilder buffer = ZString.CreateUtf8StringBuilder();
        for (var index = 0; index < value.Length; index++)
        {
            char current = value[index];

            if (char.IsDigit(current) || current is '.' or '-')
            {
                buffer.Append(current);
                if (index < value.Length - 1)
                {
                    continue;
                }
            }

            if (current != ' ')
            {
                continue;
            }

            ReadOnlySpan<byte> span = buffer.AsSpan();
            var floatValue = span.ToSingle();

            switch (++spaceCount)
            {
                case 1:
                    x = floatValue;
                    break;
                case 2:
                    y = floatValue;
                    break;
            }

            buffer.Clear();
        }

        return new Vector2(x, y);
    }

    public static Vector3 ToVector3(this ReadOnlySpan<char> value)
    {
        float x = 0;
        float y = 0;
        float z = 0;
        byte spaceCount = 0;

        using Utf8ValueStringBuilder buffer = ZString.CreateUtf8StringBuilder();
        for (var index = 0; index < value.Length; index++)
        {
            char current = value[index];

            if (char.IsDigit(current) || current is '.' or '-')
            {
                buffer.Append(current);
                if (index < value.Length - 1)
                {
                    continue;
                }
            }

            if (current != ' ')
            {
                continue;
            }

            ReadOnlySpan<byte> span = buffer.AsSpan();
            var floatValue = span.ToSingle();

            switch (++spaceCount)
            {
                case 1:
                    x = floatValue;
                    break;
                case 2:
                    y = floatValue;
                    break;
                case 3:
                    z = floatValue;
                    break;
            }

            buffer.Clear();
        }

        return new Vector3(x, y, z);
    }

    public static Vector3d ToVector3d(this ReadOnlySpan<char> value)
    {
        double x = 0;
        double y = 0;
        double z = 0;
        byte spaceCount = 0;

        using Utf8ValueStringBuilder buffer = ZString.CreateUtf8StringBuilder();
        for (var index = 0; index < value.Length; index++)
        {
            char current = value[index];

            if (char.IsDigit(current) || current is '.' or '-')
            {
                buffer.Append(current);
                if (index < value.Length - 1)
                {
                    continue;
                }
            }

            if (current != ' ')
            {
                continue;
            }

            ReadOnlySpan<byte> span = buffer.AsSpan();
            var floatValue = span.ToDouble();

            switch (++spaceCount)
            {
                case 1:
                    x = floatValue;
                    break;
                case 2:
                    y = floatValue;
                    break;
                case 3:
                    z = floatValue;
                    break;
            }

            buffer.Clear();
        }

        return new Vector3d(x, y, z);
    }

    public static Vector4 ToVector4(this ReadOnlySpan<char> value)
    {
        float x = 0;
        float y = 0;
        float z = 0;
        float w = 0;
        byte spaceCount = 0;

        using Utf8ValueStringBuilder buffer = ZString.CreateUtf8StringBuilder();
        for (var index = 0; index < value.Length; index++)
        {
            char current = value[index];

            if (char.IsDigit(current) || current is '.' or '-')
            {
                buffer.Append(current);
                if (index < value.Length - 1)
                {
                    continue;
                }
            }

            if (current != ' ')
            {
                continue;
            }

            ReadOnlySpan<byte> span = buffer.AsSpan();
            var floatValue = span.ToSingle();

            switch (++spaceCount)
            {
                case 1:
                    x = floatValue;
                    break;
                case 2:
                    y = floatValue;
                    break;
                case 3:
                    z = floatValue;
                    break;
                case 4:
                    w = floatValue;
                    break;
            }

            buffer.Clear();
        }

        return new Vector4(x, y, z, w);
    }
}
