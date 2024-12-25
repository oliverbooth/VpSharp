namespace VpSharp.Building.Serialization.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="bool" />.
/// </summary>
public sealed class YesNoBooleanValueConverter : ValueConverter<bool>
{
    /// <inheritdoc />
    public override bool Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return false;
        }

        success = true;
        ReadOnlySpan<char> span = token.ValueSpan;

        return span.Equals("yes", StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, bool value, ActionSerializerOptions options)
    {
        writer.Write(value ? "yes" : "no");
    }
}
