using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="uint" />.
/// </summary>
public sealed class UInt32ValueConverter : ValueConverter<uint>
{
    /// <inheritdoc />
    public override uint Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return 0;
        }

        success = uint.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out uint value);
        return success ? value : 0U;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, uint value, ActionSerializerOptions options)
    {
        writer.Write(value);
    }
}
