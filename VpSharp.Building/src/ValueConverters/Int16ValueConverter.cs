using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="short" />.
/// </summary>
public sealed class Int16ValueConverter : ValueConverter<short>
{
    /// <inheritdoc />
    public override short ReadValue(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = short.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out short value);
        return success ? value : (short)0;
    }
}
