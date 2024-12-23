using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="byte" />.
/// </summary>
public sealed class ByteValueConverter : ValueConverter<byte>
{
    /// <inheritdoc />
    public override byte Read(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = byte.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out byte value);
        return success ? value : (byte)0;
    }
}
