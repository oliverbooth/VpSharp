using System.Text.RegularExpressions;

namespace VpSharp.Building.Serialization.ValueConverters;

/// <summary>
///     Represents a value converter for <see cref="string" />.
/// </summary>
public sealed partial class StringValueConverter : ValueConverter<string>
{
    private static readonly Regex SpaceRegex = GetSpaceRegex();

    /// <inheritdoc />
    public override string Read(ref Utf8ActionReader reader, out bool success, ActionSerializerOptions options)
    {
        Token token = reader.Read();
        if (token.Type == TokenType.None)
        {
            success = false;
            return string.Empty;
        }

        success = true;
        return token.Value;
    }

    /// <inheritdoc />
    public override void Write(Utf8ActionWriter writer, string value, ActionSerializerOptions options)
    {
        if (SpaceRegex.IsMatch(value))
        {
            writer.Write($"\"{value}\"");
            return;
        }

        writer.Write(value);
    }

    [GeneratedRegex(@"\s", RegexOptions.Compiled)]
    private static partial Regex GetSpaceRegex();
}
