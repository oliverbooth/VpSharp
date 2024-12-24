using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="long" />.
/// </summary>
public sealed class Int64ValueConverter : ValueConverter<long>
{
    /// <inheritdoc />
    public override long Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return 0;
        }

        success = long.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out long value);
        return success ? value : 0L;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, long value, ActionSerializerOptions options)
    {
        writer.Write(value);
    }
}
