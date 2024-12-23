using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="decimal" />.
/// </summary>
public sealed class DecimalValueConverter : ValueConverter<decimal>
{
    /// <inheritdoc />
    public override decimal Read(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = decimal.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out decimal value);
        return success ? value : 0.0m;
    }
}
