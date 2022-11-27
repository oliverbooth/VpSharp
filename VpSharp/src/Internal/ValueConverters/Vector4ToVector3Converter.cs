using System.Numerics;
using Cysharp.Text;
using VpSharp.Extensions;

namespace VpSharp.Internal.ValueConverters;

internal sealed class Vector4ToVector3Converter : ValueConverter<Vector3>
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out Vector3 result)
    {
        using var builder = new Utf8ValueStringBuilder(false);
        var spaceCount = 0;

        while (true)
        {
            int readChar = reader.Read();

            var currentChar = (char) readChar;
            if (currentChar == ' ')
                spaceCount++;

            if (spaceCount < 3 && readChar != -1)
                continue;

            result = builder.AsSpan().ToVector3();
            break;
        }
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, Vector3 value)
    {
        writer.Write(value.X);
        writer.Write(' ');
        writer.Write(value.Y);
        writer.Write(' ');
        writer.Write(value.Z);
        writer.Flush();
    }
}