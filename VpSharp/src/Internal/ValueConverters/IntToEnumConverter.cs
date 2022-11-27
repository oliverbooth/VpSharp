namespace VpSharp.Internal.ValueConverters;

internal sealed class IntToEnumConverter<T> : ValueConverter<T>
    where T : struct, Enum
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out T result)
    {
        int value = int.Parse(reader.ReadToEnd());
        result = (T)Convert.ChangeType(value, typeof(T));
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, T value)
    {
        writer.Write(Convert.ToInt32(value));
    }
}