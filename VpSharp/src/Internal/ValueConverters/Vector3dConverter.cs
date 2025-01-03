﻿using Cysharp.Text;
using VpSharp.Extensions;

namespace VpSharp.Internal.ValueConverters;

#pragma warning disable CA1812

internal sealed class Vector3dConverter : ValueConverter<Vector3d>
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out Vector3d result)
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

            result = builder.AsSpan().ToVector3d();
            break;
        }
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, Vector3d value)
    {
        writer.Write($"{value.X} {value.Y} {value.Z}");
    }
}
