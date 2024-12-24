namespace VpSharp.Building.Commands.Converters;

/// <summary>
///     Represents a command converter for the <see cref="ShearCommand" /> command.
/// </summary>
public sealed class ShearCommandConverter : CommandConverter<ShearCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, ShearCommand command, ActionSerializerOptions options)
    {
        double x, y, z;

        x = y = z = 0.0;
        ReadVector(ref reader, ref z, ref x, ref y);
        command.PositiveShear = new Vector3d(x, y, z);

        x = y = z = 0.0;
        ReadVector(ref reader, ref y, ref z, ref x);
        command.NegativeShear = new Vector3d(x, y, z);
    }

    private static void ReadVector(ref Utf8ActionReader reader, ref double a, ref double b, ref double c)
    {
        Token token;
        if ((token = reader.Read()).Type != TokenType.None && !double.TryParse(token.ValueSpan, out a))
        {
            return;
        }

        if ((token = reader.Read()).Type != TokenType.None && !double.TryParse(token.ValueSpan, out b))
        {
            return;
        }

        if ((token = reader.Read()).Type != TokenType.None)
        {
            double.TryParse(token.ValueSpan, out c);
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, ShearCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        if (command.PositiveShear != Vector3d.Zero)
        {
            writer.Write(command.PositiveShear.Z);
            writer.Write(command.PositiveShear.X);
            writer.Write(command.PositiveShear.Y);
        }

        if (command.NegativeShear != Vector3d.Zero)
        {
            writer.Write(command.NegativeShear.Y);
            writer.Write(command.NegativeShear.Z);
            writer.Write(command.NegativeShear.X);
        }

        WriteProperties(writer, command, options);
    }
}
