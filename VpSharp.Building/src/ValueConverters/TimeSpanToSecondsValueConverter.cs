namespace VpSharp.Building.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="TimeSpan" /> to seconds.
/// </summary>
public sealed class TimeSpanToSecondsValueConverter : ValueConverter<TimeSpan>
{
    /// <inheritdoc />
    public override TimeSpan Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        success = token.Type != TokenType.None;

        if (token.Type is TokenType.Number or TokenType.Text && double.TryParse(token.ValueSpan, out var seconds))
        {
            return TimeSpan.FromSeconds(seconds);
        }

        return TimeSpan.Zero;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, TimeSpan value, ActionSerializerOptions options)
    {
        writer.WriteNumber(value.TotalSeconds);
    }
}
