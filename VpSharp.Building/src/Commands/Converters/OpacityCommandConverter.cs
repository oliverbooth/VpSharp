using System.Globalization;

namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="OpacityCommand" /> command.
/// </summary>
public sealed class OpacityCommandConverter : CommandConverter<OpacityCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf16ValueStringReader reader, OpacityCommand command, ActionSerializerOptions options)
    {
        Span<char> token = stackalloc char[50];
        int read = reader.ReadToken(token);
        token = token[..read];

        if (read == 0)
        {
            return;
        }

        if (double.TryParse(token, CultureInfo.InvariantCulture, out double opacity))
        {
            command.Opacity = opacity;
        }
    }

    /// <inheritdoc />
    public override void Write(TextWriter writer, OpacityCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.Write(command.Opacity.ToString(CultureInfo.InvariantCulture));
    }
}
