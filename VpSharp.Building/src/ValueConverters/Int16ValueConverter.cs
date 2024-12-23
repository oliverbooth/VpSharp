using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="short" />.
/// </summary>
public sealed class Int16ValueConverter : ValueConverter<short>
{
    /// <inheritdoc />
    public override short Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type is not TokenType.Number)
        {
            success = false;
            return 0;
        }

        success = short.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out short value);
        return success ? value : (short)0;
    }
}