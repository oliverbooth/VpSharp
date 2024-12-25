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
        float x = ReadSingle(ref reader, out success);
        if (!success)
        {
            return Vector3.Zero;
        }

        float y = ReadSingle(ref reader, out success);
        if (!success)
        {
            return Vector3.Zero;
        }

        float z = ReadSingle(ref reader, out success);
        if (!success)
        {
            return Vector3.Zero;
        }

        return new Vector3(x, y, z);
    }

    private static float ReadSingle(ref Utf8ActionReader reader, out bool success)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return 0.0f;
        }

        success = float.TryParse(token.ValueSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out float value);
        return success ? value : 0.0f;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, Vector3 value, ActionSerializerOptions options)
    {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
    }
}
