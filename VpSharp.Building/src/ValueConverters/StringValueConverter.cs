namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="string" />.
/// </summary>
public sealed class StringValueConverter : ValueConverter<string>
{
    /// <inheritdoc />
    public override string Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return string.Empty;
        }

        success = true;
        return token.Value;
    }
}
