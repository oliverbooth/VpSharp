namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="NameCommand" /> command.
/// </summary>
public sealed class NameCommandConverter : CommandConverter<NameCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf16ValueStringReader reader, NameCommand command, ActionSerializerOptions options)
    {
        Span<char> token = stackalloc char[50];
        int read = reader.ReadToken(token);
        token = token[..read];

        if (read == 0)
        {
            return;
        }

        command.Name = token.ToString();
    }

    /// <inheritdoc />
    public override void Write(TextWriter writer, NameCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.Write(command.Name);
    }
}
