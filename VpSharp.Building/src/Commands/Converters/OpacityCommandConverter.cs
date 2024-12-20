using System.Globalization;

namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="OpacityCommand" /> command.
/// </summary>
public sealed class OpacityCommandConverter : CommandConverter<OpacityCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, OpacityCommand command, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type != TokenType.Number)
        {
            return;
        }

        while (ReadProperty(ref reader, command))
        {
            // do nothing
        }

        if (double.TryParse(token.ValueSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out double opacity))
        {
            command.Opacity = opacity;
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, OpacityCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.WriteNumber(command.Opacity);
    }
}
