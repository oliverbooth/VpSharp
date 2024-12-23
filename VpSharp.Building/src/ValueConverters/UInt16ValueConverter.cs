using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="ushort" />.
/// </summary>
public sealed class UInt16ValueConverter : ValueConverter<ushort>
{
    /// <inheritdoc />
    public override ushort Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type is not TokenType.Number)
        {
            success = false;
            return 0;
        }

        success = ushort.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out ushort value);
        return success ? value : (ushort)0U;
    }
}
