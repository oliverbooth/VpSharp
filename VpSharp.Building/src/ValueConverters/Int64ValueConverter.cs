using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="long" />.
/// </summary>
public sealed class Int64ValueConverter : ValueConverter<long>
{
    /// <inheritdoc />
    public override long ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return long.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out long value) ? value : 0L;
    }
}
