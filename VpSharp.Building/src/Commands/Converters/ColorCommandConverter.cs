using System.Drawing;
using System.Globalization;

namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="NameCommand" /> command.
/// </summary>
public sealed class ColorCommandConverter : CommandConverter<ColorCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, ColorCommand command, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            return;
        }

        Color color = default;
        ReadOnlySpan<char> span = token.ValueSpan;

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

        command.Color = color;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, ColorCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        Color color = command.Color;
        writer.Write(color.IsNamedColor ? color.Name : $"#{color.ToArgb():X6}");
    }
}
