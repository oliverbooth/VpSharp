using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="int" />.
/// </summary>
public sealed class Int32ValueConverter : ValueConverter<int>
{
    /// <inheritdoc />
    public override int ReadValue(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = int.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out int value);
        return success ? value : 0;
    }
}
