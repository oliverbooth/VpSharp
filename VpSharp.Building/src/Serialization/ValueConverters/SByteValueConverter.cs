using System.Globalization;

namespace VpSharp.Building.Serialization.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="sbyte" />.
/// </summary>
public sealed class SByteValueConverter : ValueConverter<sbyte>
{
    /// <inheritdoc />
    public override sbyte Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return 0;
        }

        success = sbyte.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out sbyte value);
        return success ? value : (sbyte)0;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, sbyte value, ActionSerializerOptions options)
    {
        writer.Write(value);
    }
}
