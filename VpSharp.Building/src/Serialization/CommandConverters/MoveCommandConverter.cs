using VpSharp.Building.Commands;

namespace VpSharp.Building.Serialization.CommandConverters;

/// <summary>
///     Represents a command converter for the <see cref="MoveCommand" /> command.
/// </summary>
public sealed class MoveCommandConverter : CommandConverter<MoveCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, MoveCommand command, ActionSerializerOptions options)
    {
        Span<double> values = stackalloc double[3];
        int count = 0;

        while (count < 3 &&
               reader.Read().Type != TokenType.None &&
               double.TryParse(reader.CurrentToken.ValueSpan, out values[count]))
        {
            count++;
        }

        command.Movement = count switch
        {
            1 => new Vector3d(values[0], 0, 0),
            2 => new Vector3d(values[0], values[1], 0),
            3 => new Vector3d(values[0], values[1], values[2]),
            _ => command.Movement
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, MoveCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        switch (command.Movement)
        {
            case { X: 0.0, Z: 0.0 }:
                writer.WriteNumber(command.Movement.X);
                break;

            case { Z: 0.0 }:
                writer.WriteNumber(command.Movement.X);
                writer.WriteNumber(command.Movement.Y);
                break;

            default:
                writer.WriteNumber(command.Movement.X);
                writer.WriteNumber(command.Movement.Y);
                writer.WriteNumber(command.Movement.Z);
                break;
        }
    }
}
