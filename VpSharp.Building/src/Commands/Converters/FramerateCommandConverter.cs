using System.Globalization;

namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="FramerateCommand" /> command.
/// </summary>
public sealed class FramerateCommandConverter : CommandConverter<FramerateCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf16ValueStringReader reader, FramerateCommand command, ActionSerializerOptions options)
    {
        Span<char> token = stackalloc char[50];
        int read = reader.ReadToken(token);
        token = token[..read];

        if (read == 0)
        {
            return;
        }

        if (int.TryParse(token, CultureInfo.InvariantCulture, out int framerate))
        {
            command.Framerate = framerate;
        }
    }

    /// <inheritdoc />
    public override void Write(TextWriter writer, FramerateCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.Write(command.Framerate.ToString(CultureInfo.InvariantCulture));
    }
}
