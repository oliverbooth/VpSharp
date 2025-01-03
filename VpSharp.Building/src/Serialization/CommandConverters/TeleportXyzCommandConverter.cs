using System.Globalization;
using VpSharp.Building.Commands;

namespace VpSharp.Building.Serialization.CommandConverters;

/// <summary>
///     Represents a command converter for <see cref="TeleportXyzCommand" />.
/// </summary>
public sealed class TeleportXyzCommandConverter : CommandConverter<TeleportXyzCommand>
{
    /// <inheritdoc />
    public override void Read(ref Utf8ActionReader reader, TeleportXyzCommand command, ActionSerializerOptions options)
    {
        if (!TryReadDouble(ref reader, out double x))
        {
            return;
        }

        if (!TryReadDouble(ref reader, out double y))
        {
            return;
        }

        if (!TryReadDouble(ref reader, out double z))
        {
            return;
        }

        command.Destination = new Vector3d(x, y, z);

        if (TryReadDouble(ref reader, out double yaw))
        {
            command.Yaw = yaw;
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, TeleportXyzCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        writer.Write(command.Destination.X);
        writer.Write(command.Destination.Y);
        writer.Write(command.Destination.Z);

        if (command.Yaw != 0.0)
        {
            writer.Write(command.Yaw);
        }
    }

    private static bool TryReadDouble(ref Utf8ActionReader reader, out double value)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            value = 0.0;
            return false;
        }

        return double.TryParse(token.ValueSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }
}
