using System.Reflection;
using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents a command converter.
/// </summary>
public abstract partial class CommandConverter
{
    /// <summary>
    ///     Returns a value indicating whether this converter can convert the specified type.
    /// </summary>
    /// <param name="type">The type to convert.</param>
    /// <returns>
    ///     <see langword="true" /> if this converter can convert the specified type; otherwise, <see langword="false" />.
    /// </returns>
    public abstract bool CanConvert(Type type);

    /// <summary>
    ///     Reads the command from the specified text writer.
    /// </summary>
    /// <param name="reader">The reader from which the command will be read.</param>
    /// <param name="type">The type of the command.</param>
    /// <param name="command">The command to read into.</param>
    /// <param name="options">
    ///     An <see cref="ActionSerializerOptions" /> object which can customize the deserialization behaviour.
    /// </param>
    public abstract void Read(ref Utf16ValueStringReader reader, Type type, VirtualParadiseCommand command,
        ActionSerializerOptions options);

    /// <summary>
    ///     Writes the command to the specified text writer.
    /// </summary>
    /// <param name="writer">The writer to which the command will be written.</param>
    /// <param name="type">The type of the command.</param>
    /// <param name="command">The command to write.</param>
    /// <param name="options">
    ///     An <see cref="ActionSerializerOptions" /> object which can customize the serialization behaviour.
    /// </param>
    public abstract void Write(TextWriter writer, Type type, VirtualParadiseCommand? command, ActionSerializerOptions options);
}

/// <summary>
///     Represents a command converter.
/// </summary>
public abstract class CommandConverter<T> : CommandConverter
    where T : VirtualParadiseCommand
{
    /// <inheritdoc />
    public override bool CanConvert(Type type)
    {
        return typeof(T).IsAssignableFrom(type);
    }

    /// <summary>
    ///     Reads the command from the specified text writer.
    /// </summary>
    /// <param name="reader">The reader from which the command will be read.</param>
    /// <param name="command">The command to read into.</param>
    /// <param name="options">
    ///     An <see cref="ActionSerializerOptions" /> object which can customize the deserialization behaviour.
    /// </param>
    /// <returns>The command that was read from <paramref name="reader" />.</returns>
    public abstract void Read(ref Utf16ValueStringReader reader, T command, ActionSerializerOptions options);

    /// <inheritdoc />
    public override void Read(ref Utf16ValueStringReader reader, Type type, VirtualParadiseCommand command,
        ActionSerializerOptions options)
    {
        if (!CanConvert(type))
        {
            return;
        }

        Read(ref reader, (T)command, options);
    }

    /// <summary>
    ///     Writes the command to the specified text writer.
    /// </summary>
    /// <param name="writer">The writer to which the command will be written.</param>
    /// <param name="command">The command to write.</param>
    /// <param name="options">
    ///     An <see cref="ActionSerializerOptions" /> object which can customize the serialization behaviour.
    /// </param>
    public abstract void Write(TextWriter writer, T? command, ActionSerializerOptions options);

    /// <inheritdoc />
    public override void Write(TextWriter writer, Type type, VirtualParadiseCommand? command, ActionSerializerOptions options)
    {
        if (command is null)
        {
            return;
        }

        if (!CanConvert(type))
        {
            return;
        }

        Write(writer, command as T, options);
    }

    /// <summary>
    ///     Writes the properties of the command to the specified text writer.
    /// </summary>
    /// <param name="writer">The writer to which the properties will be written.</param>
    /// <param name="command">The command whose properties will be written.</param>
    /// <param name="options">
    ///     An <see cref="ActionSerializerOptions" /> object which can customize the serialization behaviour.
    /// </param>
    /// <typeparam name="T">The type of the command.</typeparam>
    protected void WriteProperties(TextWriter writer, T command, ActionSerializerOptions options)
    {
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<PropertyAttribute>() is not { } attribute)
            {
                continue;
            }

            object? value = property.GetValue(command);
            if (value is null)
            {
                continue;
            }

            writer.Write($" {attribute.Name}={value}");
        }
    }
}
