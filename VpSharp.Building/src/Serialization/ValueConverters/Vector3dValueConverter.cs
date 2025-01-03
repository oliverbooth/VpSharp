using System.Globalization;
using System.Numerics;

namespace VpSharp.Building.Serialization.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="Vector3" />.
/// </summary>
public sealed class Vector3dValueConverter : ValueConverter<Vector3d>
{
    /// <inheritdoc />
    public override Vector3d Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        success = false;

        if (!TryReadDouble(ref reader, out double x))
        {
            return Vector3d.Zero;
        }

        if (!TryReadDouble(ref reader, out double y))
        {
            return Vector3d.Zero;
        }

        if (!TryReadDouble(ref reader, out double z))
        {
            return Vector3d.Zero;
        }

        success = true;
        return new Vector3d(x, y, z);
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, Vector3d value, ActionSerializerOptions options)
    {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
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
