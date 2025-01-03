namespace VpSharp.Internal.ValueConverters;

[Obsolete]
internal abstract class ValueConverter
{
    public abstract void Deserialize(TextReader reader, out object result);

    public abstract void Serialize(TextWriter writer, object value);
}

#pragma warning disable 612
internal abstract class ValueConverter<T> : ValueConverter
#pragma warning restore 612
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out object result)
    {
        Deserialize(reader, out T actual);
        result = actual!;
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, object value)
    {
        Serialize(writer, (T)value);
    }

    public abstract void Deserialize(TextReader reader, out T result);

    public abstract void Serialize(TextWriter writer, T value);
}
