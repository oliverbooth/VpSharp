using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="byte" />.
/// </summary>
public sealed class ByteValueConverter : ValueConverter<byte>
{
    /// <inheritdoc />
    public override byte Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return 0;
        }

        success = byte.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out byte value);
        return success ? value : (byte)0;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, byte value, ActionSerializerOptions options)
    {
        writer.Write(value);
    }
}
