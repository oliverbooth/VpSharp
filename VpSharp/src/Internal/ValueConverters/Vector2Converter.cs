using System.Numerics;
using Cysharp.Text;
using VpSharp.Extensions;

namespace VpSharp.Internal.ValueConverters;

#pragma warning disable CA1812

internal sealed class Vector2Converter : ValueConverter<Vector2>
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out Vector2 result)
    {
        using var builder = new Utf8ValueStringBuilder(false);
        var spaceCount = 0;

        while (true)
        {
            int readChar = reader.Read();

            var currentChar = (char)readChar;
            if (currentChar == ' ')
            {
                spaceCount++;
            }

            if (spaceCount < 2 && readChar != -1)
            {
                continue;
            }

            result = builder.AsSpan().ToVector2();
            break;
        }
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, Vector2 value)
    {
        writer.Write($"{value.X} {value.Y}");
    }
}
