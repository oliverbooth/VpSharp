using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="float" />.
/// </summary>
public sealed class SingleValueConverter : ValueConverter<float>
{
    /// <inheritdoc />
    public override float Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type is not TokenType.Number)
        {
            success = false;
            return 0;
        }

        success = float.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out float value);
        return success ? value : 0.0f;
    }
}
