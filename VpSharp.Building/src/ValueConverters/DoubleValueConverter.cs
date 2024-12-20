using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="double" />.
/// </summary>
public sealed class DoubleValueConverter : ValueConverter<double>
{
    /// <inheritdoc />
    public override double ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return double.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out double value) ? value : 0.0;
    }
}
