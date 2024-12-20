using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="Half" />.
/// </summary>
public sealed class HalfValueConverter : ValueConverter<Half>
{
    /// <inheritdoc />
    public override Half ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return Half.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out Half value) ? value : (Half)0.0f;
    }
}
