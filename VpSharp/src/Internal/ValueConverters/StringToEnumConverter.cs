using System.Reflection;
using VpSharp.Internal.Attributes;

namespace VpSharp.Internal.ValueConverters;

#pragma warning disable CA1812

internal sealed class StringToEnumConverter<T> : ValueConverter<T>
    where T : struct, Enum
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out T result)
    {
        string value = reader.ReadToEnd();

        FieldInfo? field = typeof(T).GetFields().FirstOrDefault(f =>
            string.Equals(f.GetCustomAttribute<SerializationKeyAttribute>()?.Key, value, StringComparison.Ordinal));

        if (field is not null)
        {
            result = (T)field.GetValue(Enum.GetValues<T>()[0])!;
        }
        else
        {
            result = Enum.Parse<T>(value, true);
        }
    }

    /// <inheritdoc />
    public override void Serialize(TextWriter writer, T value)
    {
#pragma warning disable CA1308
        writer.Write(value.ToString().ToLowerInvariant());
#pragma warning restore CA1308
    }
}
