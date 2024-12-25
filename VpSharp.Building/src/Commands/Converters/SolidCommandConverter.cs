namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="SolidCommand" /> command.
/// </summary>
public sealed class SolidCommandConverter : CommandConverter<SolidCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, SolidCommand command, ActionSerializerOptions options)
    {
        reader.Read();

        if (command.RawArguments.Count > 1)
        {
            command.Target = reader.CurrentToken.Value;
            reader.Read();
        }

        try
        {
            command.IsSolid = reader.GetBoolean();
        }
        catch
        {
            // ignored
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
    }
}
