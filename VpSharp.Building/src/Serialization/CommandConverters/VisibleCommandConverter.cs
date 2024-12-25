using VpSharp.Building.Commands;

namespace VpSharp.Building.Serialization.CommandConverters;

/// <summary>
///     Represents a command converter for the <see cref="VisibleCommand" /> command.
/// </summary>
public sealed class VisibleCommandConverter : CommandConverter<VisibleCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, VisibleCommand command, ActionSerializerOptions options)
    {
        reader.Read();

        if (command.RawArguments.Count > 1)
        {
            command.Target = reader.CurrentToken.Value;
            reader.Read();
        }

        try
        {
            command.IsVisible = reader.GetBoolean();
        }
        catch
        {
            // ignored
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
    }
}
