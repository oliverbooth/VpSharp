using System.Globalization;

namespace VpSharp.Building.Serialization.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="decimal" />.
/// </summary>
public sealed class DecimalValueConverter : ValueConverter<decimal>
{
    /// <inheritdoc />
    public override decimal Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return 0;
        }

        success = decimal.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out decimal value);
        return success ? value : 0.0m;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, decimal value, ActionSerializerOptions options)
    {
        writer.Write(value);
    }
}
