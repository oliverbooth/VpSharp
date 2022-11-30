using Cysharp.Text;
using VpSharp.Extensions;

namespace VpSharp.Internal.ValueConverters;

#pragma warning disable CA1812

internal sealed class Vector3ToColorConverter : ValueConverter<ColorF>
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out ColorF result)
    {
        using Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();
        var spaceCount = 0;

        while (true)
        {
            int readChar = reader.Read();

            var currentChar = (char)readChar;
            if (currentChar == ' ')
            {
                spaceCount++;
            }

            if (spaceCount < 3 && readChar != -1)
            {
                continue;
            }

            (float x, float y, float z) = builder.AsSpan().ToVector3();
            result = ColorF.FromArgb(x, y, z);
            break;
        }
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, ColorF value)
    {
        writer.Write($"{value.R} {value.G} {value.B} {value.A}");
    }
}
