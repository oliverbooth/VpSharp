using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="ulong" />.
/// </summary>
public sealed class UInt64ValueConverter : ValueConverter<ulong>
{
    /// <inheritdoc />
    public override ulong ReadValue(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = ulong.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out ulong value);
        return success ? value : 0UL;
    }
}
