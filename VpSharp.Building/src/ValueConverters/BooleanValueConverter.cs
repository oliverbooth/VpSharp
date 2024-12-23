namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="bool" />.
/// </summary>
public sealed class BooleanValueConverter : ValueConverter<bool>
{
    /// <inheritdoc />
    public override bool Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type is not (TokenType.Text or TokenType.Number))
        {
            success = false;
            return false;
        }

        ReadOnlySpan<char> span = token.ValueSpan;

        if (span.Equals("on", StringComparison.OrdinalIgnoreCase) ||
            span.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
            span.Equals("1", StringComparison.OrdinalIgnoreCase))
        {
            success = true;
            return true;
        }

        if (span.Equals("off", StringComparison.OrdinalIgnoreCase) ||
            span.Equals("no", StringComparison.OrdinalIgnoreCase) ||
            span.Equals("0", StringComparison.OrdinalIgnoreCase))
        {
            success = true;
            return false;
        }

        success = false;
        return false;
    }
}