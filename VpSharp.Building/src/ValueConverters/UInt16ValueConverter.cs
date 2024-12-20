using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="ushort" />.
/// </summary>
public sealed class UInt16ValueConverter : ValueConverter<ushort>
{
    /// <inheritdoc />
    public override ushort ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return ushort.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out ushort value) ? value : (ushort)0U;
    }
}
