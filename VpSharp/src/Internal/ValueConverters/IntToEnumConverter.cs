using System.Globalization;

namespace VpSharp.Internal.ValueConverters;

#pragma warning disable CA1812

internal sealed class IntToEnumConverter<T> : ValueConverter<T>
    where T : struct, Enum
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out T result)
    {
        int value = int.Parse(reader.ReadToEnd(), provider: CultureInfo.InvariantCulture);
        result = (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, T value)
    {
        writer.Write(Convert.ToInt32(value, CultureInfo.InvariantCulture));
    }
}
