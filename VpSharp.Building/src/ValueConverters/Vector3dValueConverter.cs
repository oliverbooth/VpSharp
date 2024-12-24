using System.Globalization;
using System.Numerics;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="Vector3" />.
/// </summary>
public sealed class Vector3dValueConverter : ValueConverter<Vector3d>
{
    /// <inheritdoc />
    public override Vector3d Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        double x = ReadDouble(ref reader, out success);
        if (!success)
        {
            return Vector3d.Zero;
        }

        double y = ReadDouble(ref reader, out success);
        if (!success)
        {
            return Vector3d.Zero;
        }

        double z = ReadDouble(ref reader, out success);
        if (!success)
        {
            return Vector3d.Zero;
        }

        return new Vector3d(x, y, z);
    }

    private static double ReadDouble(ref Utf8ActionReader reader, out bool success)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return 0.0;
        }

        success = double.TryParse(token.ValueSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out double value);
        return success ? value : 0.0;
    }
}
