namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="char" />.
/// </summary>
public sealed class CharValueConverter : ValueConverter<char>
{
    /// <inheritdoc />
    public override char Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return '\0';
        }

        ReadOnlySpan<char> span = token.ValueSpan;
        if (span.Length == 1)
        {
            success = true;
            return span[0];
        }

        success = false;
        return '\0';
    }
}
