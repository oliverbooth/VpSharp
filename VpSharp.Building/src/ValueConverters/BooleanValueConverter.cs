namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="bool" />.
/// </summary>
public sealed class BooleanValueConverter : ValueConverter<bool>
{
    /// <inheritdoc />
    public override bool ReadValue(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        if (reader.Peek() == -1)
        {
            success = false;
            return false;
        }

        ReadOnlySpan<char> span = reader.ReadToEnd();
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
