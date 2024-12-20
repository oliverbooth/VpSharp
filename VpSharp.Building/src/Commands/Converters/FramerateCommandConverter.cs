using System.Globalization;

namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="FramerateCommand" /> command.
/// </summary>
public sealed class FramerateCommandConverter : CommandConverter<FramerateCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, FramerateCommand command, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type != TokenType.None)
        {
            return;
        }

        if (int.TryParse(token.ValueSpan, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
        {
            command.Framerate = value;
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, FramerateCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.WriteNumber(command.Framerate);
    }
}
