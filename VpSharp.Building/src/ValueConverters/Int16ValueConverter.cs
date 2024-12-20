using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="short" />.
/// </summary>
public sealed class Int16ValueConverter : ValueConverter<short>
{
    /// <inheritdoc />
    public override short ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return short.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out short value) ? value : (short)0;
    }
}
