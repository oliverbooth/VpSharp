using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="Half" />.
/// </summary>
public sealed class HalfValueConverter : ValueConverter<Half>
{
    /// <inheritdoc />
    public override Half Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return (Half)0.0f;
        }

        success = Half.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out Half value);
        return success ? value : (Half)0.0f;
    }
}
