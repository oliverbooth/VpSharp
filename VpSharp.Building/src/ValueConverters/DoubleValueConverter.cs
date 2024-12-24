using System.Globalization;

namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="double" />.
/// </summary>
public sealed class DoubleValueConverter : ValueConverter<double>
{
    /// <inheritdoc />
    public override double Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return 0;
        }

        success = double.TryParse(token.ValueSpan, CultureInfo.InvariantCulture, out double value);
        return success ? value : 0.0;
    }
}
