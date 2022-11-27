using Cysharp.Text;
using VpSharp.Extensions;

namespace VpSharp.Internal.ValueConverters;

internal sealed class Vector4ToColorConverter : ValueConverter<ColorF>
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out ColorF result)
    {
        using var builder = new Utf8ValueStringBuilder(false);
        var spaceCount = 0;

        while (true)
        {
            int readChar = reader.Read();

            var currentChar = (char) readChar;
            if (currentChar == ' ')
            {
                spaceCount++;
            }

            if (spaceCount < 4 && readChar != -1)
            {
                continue;
            }

            (float x, float y, float z, float w) = builder.AsSpan().ToVector4();
            result = ColorF.FromArgb(w, x, y, z);
            break;
        }
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, ColorF value)
    {
        writer.Write($"{value.R} {value.G} {value.B} {value.A}");
    }
}