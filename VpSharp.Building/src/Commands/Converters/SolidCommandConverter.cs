namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="SolidCommand" /> command.
/// </summary>
public sealed class SolidCommandConverter : CommandConverter<SolidCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, SolidCommand command, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type != TokenType.None)
        {
            return;
        }

        ReadProperty(ref reader, command);

        string potentialTarget = token.Value;

        if (reader.TryGetBoolean(out bool value))
        {
            command.IsSolid = value;
        }

        token = reader.Read();
        if (token.Type == TokenType.None)
        {
            return;
        }

        ReadProperty(ref reader, command);

        if (reader.TryGetBoolean(out value))
        {
            command.Target = potentialTarget;
            command.IsSolid = value;
        }

        while (ReadProperty(ref reader, command))
        {
            // do nothing
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, SolidCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        if (command.Target is not null)
        {
            writer.Write(command.Target);
        }

        writer.WriteBoolean(command.IsSolid);
        WriteProperties(writer, command, options);
    }
}
