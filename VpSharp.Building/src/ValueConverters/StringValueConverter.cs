namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="string" />.
/// </summary>
public sealed class StringValueConverter : ValueConverter<string>
{
    /// <inheritdoc />
    public override string ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return reader.ReadToEnd().ToString();
    }
}
