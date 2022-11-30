using Cysharp.Text;
using VpSharp.Extensions;

namespace VpSharp.Internal.ValueConverters;

#pragma warning disable CA1812

internal sealed class IntToBoolConverter : ValueConverter<bool>
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out bool result)
    {
        using Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();
        int read;
        while ((read = reader.Read()) != -1)
        {
            var current = (char)read;
            builder.Append(current);
        }

        result = builder.AsSpan().ToInt32() != 0;
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, bool value)
    {
        writer.Write(value ? 1 : 0);
    }
}
