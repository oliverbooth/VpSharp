namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="VisibleCommand" /> command.
/// </summary>
public sealed class VisibleCommandConverter : CommandConverter<VisibleCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, VisibleCommand command, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            return;
        }

        ReadProperty(ref reader, command);

        string potentialTarget = token.Value;

        if (reader.TryGetBoolean(out bool value))
        {
            command.IsVisible = value;
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
            command.IsVisible = value;
        }

        while (ReadProperty(ref reader, command))
        {
            // do nothing
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, VisibleCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        if (command.Target is not null)
        {
            writer.Write(command.Target);
        }

        writer.WriteBoolean(command.IsVisible);
        WriteProperties(writer, command, options);
    }
}
