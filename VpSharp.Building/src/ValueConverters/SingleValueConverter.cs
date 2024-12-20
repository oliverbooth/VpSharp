using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="float" />.
/// </summary>
public sealed class SingleValueConverter : ValueConverter<float>
{
    /// <inheritdoc />
    public override float ReadValue(ref Utf16ValueStringReader reader, ActionSerializerOptions options)
    {
        return float.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out float value) ? value : 0.0f;
    }
}
