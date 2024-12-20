namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="char" />.
/// </summary>
public sealed class CharValueConverter : ValueConverter<char>
{
    /// <inheritdoc />
    public override char ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return (char)reader.Read();
    }
}
