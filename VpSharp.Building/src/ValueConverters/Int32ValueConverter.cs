using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="int" />.
/// </summary>
public sealed class Int32ValueConverter : ValueConverter<int>
{
    /// <inheritdoc />
    public override int Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type is not TokenType.Number)
        {
            success = false;
            return 0;
        }

        success = int.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out int value);
        return success ? value : 0;
    }
}
