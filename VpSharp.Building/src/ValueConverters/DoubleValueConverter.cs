using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="double" />.
/// </summary>
public sealed class DoubleValueConverter : ValueConverter<double>
{
    /// <inheritdoc />
    public override double ReadValue(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = double.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out double value);
        return success ? value : 0.0;
    }
}
