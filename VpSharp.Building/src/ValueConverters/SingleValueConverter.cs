using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="float" />.
/// </summary>
public sealed class SingleValueConverter : ValueConverter<float>
{
    /// <inheritdoc />
    public override float Read(ref Utf16ValueStringReader reader, out bool success, ActionSerializerOptions options)
    {
        success = float.TryParse(reader.ReadToEnd(), CultureInfo.InvariantCulture, out float value);
        return success ? value : 0.0f;
    }
}
