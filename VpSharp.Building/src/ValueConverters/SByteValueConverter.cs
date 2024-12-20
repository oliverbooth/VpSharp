using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="sbyte" />.
/// </summary>
public sealed class SByteValueConverter : ValueConverter<sbyte>
{
    /// <inheritdoc />
    public override sbyte ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return sbyte.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out sbyte value) ? value : (sbyte)0;
    }
}
