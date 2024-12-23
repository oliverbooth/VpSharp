using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="Half" />.
/// </summary>
public sealed class HalfValueConverter : ValueConverter<Half>
{
    /// <inheritdoc />
    public override Half Read(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = Half.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out Half value);
        return success ? value : (Half)0.0f;
    }
}
