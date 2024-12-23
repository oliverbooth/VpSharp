using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="ushort" />.
/// </summary>
public sealed class UInt16ValueConverter : ValueConverter<ushort>
{
    /// <inheritdoc />
    public override ushort Read(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = ushort.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out ushort value);
        return success ? value : (ushort)0U;
    }
}
