using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="sbyte" />.
/// </summary>
public sealed class SByteValueConverter : ValueConverter<sbyte>
{
    /// <inheritdoc />
    public override sbyte Read(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = sbyte.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out sbyte value);
        return success ? value : (sbyte)0;
    }
}
