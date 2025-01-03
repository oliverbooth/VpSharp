﻿using Cysharp.Text;
using VpSharp.Extensions;

namespace VpSharp.Internal.ValueConverters;

#pragma warning disable CA1812

internal sealed class MillisecondToTimeSpanConverter : ValueConverter<TimeSpan>
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out TimeSpan result)
    {
        using Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();
        int read;
        while ((read = reader.Read()) != -1)
        {
            var current = (char)read;
            builder.Append(current);
        }

        result = TimeSpan.FromMilliseconds(builder.AsSpan().ToDouble());
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, TimeSpan value)
    {
        writer.Write(value.TotalMilliseconds);
    }
}
