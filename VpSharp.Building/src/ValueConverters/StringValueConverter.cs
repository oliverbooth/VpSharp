namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="string" />.
/// </summary>
public sealed class StringValueConverter : ValueConverter<string>
{
    /// <inheritdoc />
    public override string Read(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = reader.Peek() != -1;
        return reader.ReadToEnd().ToString();
    }
}
