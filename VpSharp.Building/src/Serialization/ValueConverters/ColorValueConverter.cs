using System.Drawing;
using System.Globalization;

namespace VpSharp.Building.Serialization.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="Color" />.
/// </summary>
public sealed class ColorValueConverter : ValueConverter<Color>
{
    /// <inheritdoc />
    public override Color Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type is not (TokenType.Text or TokenType.Number))
        {
            success = false;
            return Color.Empty;
        }

        ReadOnlySpan<char> span = token.ValueSpan;
        Color color = Color.Empty;

        if (Enum.TryParse(span, true, out KnownColor knownColor))
        {
            color = Color.FromKnownColor(knownColor);
        }
        else
        {
            if (span.Length == 7 && span[0] == '#')
            {
                span = span[1..];
            }

            if (int.TryParse(span, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int argb))
            {
                color = Color.FromArgb((int)(0xFF000000 | argb));
            }
        }

        success = color != Color.Empty;
        return color;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, Color value, ActionSerializerOptions options)
    {
        writer.Write((value.ToArgb() & 0xFFFFFF).ToString("X6"));
    }
}
