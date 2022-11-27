using System.Reflection;
using VpSharp.Internal.Attributes;

namespace VpSharp.Internal.ValueConverters;

internal sealed class StringToEnumConverter<T> : ValueConverter<T>
    where T : struct, Enum
{
    /// <inheritdoc />
    public override void Deserialize(TextReader reader, out T result)
    {
        string value = reader.ReadToEnd();
            
        FieldInfo? field = typeof(T).GetFields().FirstOrDefault(f => string.Equals(f.GetCustomAttribute<SerializationKeyAttribute>()?.Key, value));
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
        writer.Write(value.ToString().ToLowerInvariant());
    }
}