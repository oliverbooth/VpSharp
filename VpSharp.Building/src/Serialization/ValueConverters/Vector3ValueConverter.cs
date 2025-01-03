using System.Globalization;
using System.Numerics;

namespace VpSharp.Building.Serialization.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="Vector3" />.
/// </summary>
public sealed class Vector3ValueConverter : ValueConverter<Vector3>
{
    /// <inheritdoc />
    public override Vector3 Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        success = false;

        if (!TryReadSingle(ref reader, out float x))
        {
            return Vector3.Zero;
        }

        if (!TryReadSingle(ref reader, out float y))
        {
            return Vector3.Zero;
        }

        if (!TryReadSingle(ref reader, out float z))
        {
            return Vector3.Zero;
        }

        success = true;
        return new Vector3(x, y, z);
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, Vector3 value, ActionSerializerOptions options)
    {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
    }

    private static bool TryReadSingle(ref Utf8ActionReader reader, out float value)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            value = 0.0f;
            return false;
        }

        return float.TryParse(token.ValueSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }
}
