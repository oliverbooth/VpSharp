using VpSharp.Building.Commands;

namespace VpSharp.Building.Serialization.CommandConverters;

/// <summary>
///     Represents a command converter for <see cref="TeleportCommand" />.
/// </summary>
public sealed class TeleportCommandConverter : CommandConverter<TeleportCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, TeleportCommand command, ActionSerializerOptions options)
    {
        command.Coordinates = Coordinates.Parse(command.RawArgumentString);
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, TeleportCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.Write(command.Coordinates.ToString());
    }
}
