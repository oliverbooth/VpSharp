namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="NameCommand" /> command.
/// </summary>
public sealed class NameCommandConverter : CommandConverter<NameCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, NameCommand command, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        command.Name = token.Value;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, NameCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.Write(command.Name);
    }
}
