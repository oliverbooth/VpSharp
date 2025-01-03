using System.Globalization;

namespace VpSharp.Building.Serialization.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="float" />.
/// </summary>
public sealed class SingleValueConverter : ValueConverter<float>
{
    /// <inheritdoc />
    public override float Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return 0;
        }

        success = float.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out float value);
        return success ? value : 0.0f;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, float value, ActionSerializerOptions options)
    {
        writer.Write(value);
    }
}
