using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="ulong" />.
/// </summary>
public sealed class UInt64ValueConverter : ValueConverter<ulong>
{
    /// <inheritdoc />
    public override ulong ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return ulong.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out ulong value) ? value : 0UL;
    }
}
