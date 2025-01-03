using VpSharp.Building.Commands;

namespace VpSharp.Building.Serialization.CommandConverters;

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
            writer.WriteNumber(command.PositiveShear.Z);
            if (command.PositiveShear is not { X: 0.0, Y: 0.0 })
            {
                writer.WriteNumber(command.PositiveShear.X);
                if (command.PositiveShear is not { Y: 0.0 })
                {
                    writer.WriteNumber(command.PositiveShear.Y);
                }
            }
        }

        if (command.NegativeShear != Vector3d.Zero)
        {
            writer.WriteNumber(command.NegativeShear.Y);
            if (command.NegativeShear is not { X: 0.0, Z: 0.0 })
            {
                writer.WriteNumber(command.NegativeShear.Z);
                if (command.NegativeShear is not { X: 0.0 })
                {
                    writer.WriteNumber(command.NegativeShear.X);
                }
            }
        }
    }
}
