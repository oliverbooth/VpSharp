using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="uint" />.
/// </summary>
public sealed class UInt32ValueConverter : ValueConverter<uint>
{
    /// <inheritdoc />
    public override uint ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return uint.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out uint value) ? value : 0U;
    }
}
