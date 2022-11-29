using System.Drawing;
using System.Globalization;

namespace VpSharp.Internal.ValueConverters;

internal sealed class HexToColorConverter : ValueConverter<Color>
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out Color result)
    {
        Span<char> buffer = stackalloc char[2];

        reader.Read(buffer);
        int r = int.Parse(buffer, NumberStyles.HexNumber);

        reader.Read(buffer);
        int g = int.Parse(buffer, NumberStyles.HexNumber);

        reader.Read(buffer);
        int b = int.Parse(buffer, NumberStyles.HexNumber);

        result = Color.FromArgb(r, g, b);
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, Color value)
    {
        writer.Write($"{value.R:X2}{value.G:X2}{value.B:X2}");
    }
}
