using System.Globalization;

namespace VpSharp.Building.Serialization.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="ulong" />.
/// </summary>
public sealed class UInt64ValueConverter : ValueConverter<ulong>
{
    /// <inheritdoc />
    public override ulong Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return 0;
        }

        success = ulong.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out ulong value);
        return success ? value : 0UL;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, ulong value, ActionSerializerOptions options)
    {
        writer.Write(value);
    }
}
