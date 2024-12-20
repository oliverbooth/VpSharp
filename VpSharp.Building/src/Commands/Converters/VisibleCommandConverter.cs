namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="VisibleCommand" /> command.
/// </summary>
public sealed class VisibleCommandConverter : CommandConverter<VisibleCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf16ValueStringReader reader, VisibleCommand command, ActionSerializerOptions options)
    {
        Span<char> token = stackalloc char[50];
        int read = reader.ReadToken(token);
        token = token[..read];

        if (read == 0)
        {
            return;
        }

        Span<char> target = stackalloc char[read];
        token.CopyTo(target);
        token.Clear();

        read = reader.ReadToken(token);
        token = token[..read];

        command.Target = read == 0 ? null : target.ToString();
        command.IsVisible = (read == 0 ? target : token) switch
        {
            "on" or "yes" or "1" => true,
            "off" or "no" or "0" => false,
            _ => command.IsVisible
        };
    }

    /// <inheritdoc />
    public override void Write(TextWriter writer, VisibleCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        if (command.Target is not null)
        {
            writer.Write(command.Target);
            writer.Write(' ');
        }

        writer.Write(command.IsVisible ? "on" : "off");
    }
}
