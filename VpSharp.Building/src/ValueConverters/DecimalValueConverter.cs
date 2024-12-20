using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="decimal" />.
/// </summary>
public sealed class DecimalValueConverter : ValueConverter<decimal>
{
    /// <inheritdoc />
    public override decimal ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return decimal.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out decimal value) ? value : 0.0m;
    }
}
