namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="RotateCommand" /> command.
/// </summary>
public sealed class RotateCommandConverter : CommandConverter<RotateCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, RotateCommand command, ActionSerializerOptions options)
    {
        Span<double> values = stackalloc double[3];
        int count = 0;

        while (count < 3 &&
               reader.Read().Type != TokenType.None &&
               double.TryParse(reader.CurrentToken.ValueSpan, out values[count]))
        {
            count++;
        }

        command.Rotation = count switch
        {
            1 => new Vector3d(0, values[0], 0),
            2 => new Vector3d(values[0], values[1], 0),
            3 => new Vector3d(values[0], values[1], values[2]),
            _ => command.Rotation
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, RotateCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        switch (command.Rotation)
        {
            case { X: 0.0, Z: 0.0 }:
                writer.Write(command.Rotation.Y);
                break;

            case { Z: 0.0 }:
                writer.Write(command.Rotation.X);
                writer.Write(command.Rotation.Y);
                break;

            default:
                writer.Write(command.Rotation.X);
                writer.Write(command.Rotation.Y);
                writer.Write(command.Rotation.Z);
                break;
        }

        WriteProperties(writer, command, options);
    }
}
