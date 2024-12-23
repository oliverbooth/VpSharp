using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="uint" />.
/// </summary>
public sealed class UInt32ValueConverter : ValueConverter<uint>
{
    /// <inheritdoc />
    public override uint Read(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = uint.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out uint value);
        return success ? value : 0U;
    }
}
