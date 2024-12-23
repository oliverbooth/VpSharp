using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="long" />.
/// </summary>
public sealed class Int64ValueConverter : ValueConverter<long>
{
    /// <inheritdoc />
    public override long ReadValue(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = long.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out long value);
        return success ? value : 0L;
    }
}
