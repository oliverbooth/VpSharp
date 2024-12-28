using System.Reflection;
using Optional;
using VpSharp.Building.Annotations;
using VpSharp.Building.Commands;
using VpSharp.Building.Extensions;
using VpSharp.Building.Serialization.ValueConverters;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Serialization;

public partial class ActionSerializer
{
    /// <summary>
    ///     Serializes the specified <see cref="VirtualParadiseAction" /> to the specified stream.
    /// </summary>
    /// <param name="action">The action to serialize.</param>
    /// <param name="options">The options to use when serializing the action.</param>
    /// <returns>The serialized action.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
    public static string Serialize(VirtualParadiseAction action, ActionSerializerOptions? options = null)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        options ??= ActionSerializerOptions.Default;

        using var stream = new MemoryStream();
        SerializeTriggers(stream, action.Triggers, options);
        return Encoding.GetString(stream.ToArray());
    }

    /// <summary>
    ///     Serializes the specified <see cref="VirtualParadiseAction" /> to the specified stream.
    /// </summary>
    /// <param name="destinationStream">The destination stream.</param>
    /// <param name="action">The action to serialize.</param>
    /// <param name="options">The options to use when serializing the action.</param>
    /// <exception cref="ArgumentNullException">
    ///     <para><paramref name="destinationStream" /> is <see langword="null" />.</para>
    ///     -or-
    ///     <para><paramref name="action" /> is <see langword="null" />.</para>
    /// </exception>
    public static void Serialize(Stream destinationStream, VirtualParadiseAction action, ActionSerializerOptions? options = null)
    {
        if (destinationStream is null)
        {
            throw new ArgumentNullException(nameof(destinationStream));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        options ??= ActionSerializerOptions.Default;
        SerializeTriggers(destinationStream, action.Triggers, options);
    }

    private static void SerializeTriggers(Stream stream,
        IReadOnlyList<VirtualParadiseTrigger> triggers,
        ActionSerializerOptions options)
    {
        for (var index = 0; index < triggers.Count; index++)
        {
            VirtualParadiseTrigger trigger = triggers[index];

            SerializeTrigger(stream, trigger, options);

            if (index < triggers.Count - 1)
            {
                stream.Write("; "u8);
            }
        }
    }

    private static void SerializeTrigger(Stream stream, VirtualParadiseTrigger trigger, ActionSerializerOptions options)
    {
        ReadOnlySpan<char> triggerName = trigger.GetTriggerName().AsSpan();
        int byteCount = Encoding.GetByteCount(triggerName);
        Span<byte> bytes = stackalloc byte[byteCount];
        Encoding.GetBytes(triggerName, bytes);
        stream.Write(bytes);

        if (trigger.Commands.Count <= 0)
        {
            return;
        }

        stream.WriteByte((byte)' ');
        SerializeCommands(stream, trigger.Commands, options);
    }

    private static void SerializeCommands(Stream stream,
        IReadOnlyList<VirtualParadiseCommand> commands,
        ActionSerializerOptions options)
    {
        for (var index = 0; index < commands.Count; index++)
        {
            VirtualParadiseCommand command = commands[index];

            SerializeCommand(stream, command, options);

            if (index < commands.Count - 1)
            {
                stream.Write(", "u8);
            }
        }
    }

    private static void SerializeCommand(Stream stream, VirtualParadiseCommand command, ActionSerializerOptions options)
    {
        Type commandType = command.GetType();
        CommandAttribute? attribute = commandType.GetCustomAttribute<CommandAttribute>();

        if (attribute is null)
        {
            throw new InvalidOperationException($"Command {commandType.Name} is missing a {nameof(CommandAttribute)}.");
        }

        ReadOnlySpan<char> commandName = attribute.CommandName;
        int byteCount = Encoding.GetByteCount(commandName);
        Span<byte> bytes = stackalloc byte[byteCount];
        Encoding.GetBytes(commandName, bytes);
        stream.Write(bytes);

        var writer = new Utf8ActionWriter(stream);

        if (attribute.ConverterType is null)
        {
            SerializeCommand(writer, command, options);
            return;
        }

        if (Activator.CreateInstance(attribute.ConverterType) is CommandConverter converter)
        {
            if (converter.CanConvert(commandType))
            {
                converter.Write(writer, commandType, command, options);
                SerializeCommand(writer, command, options, true);
            }
        }
    }

    private static void SerializeCommand(Utf8ActionWriter writer, VirtualParadiseCommand command, ActionSerializerOptions options,
        bool skipParameters = false)
    {
        Type commandType = command.GetType();
        PropertyInfo[] members = commandType.GetProperties(PropertyBindingFlags);

        var parameters = new List<PropertyInfo>();
        var flags = new List<PropertyInfo>();
        var properties = new List<PropertyInfo>();

        foreach (PropertyInfo member in members)
        {
            if (!skipParameters && member.GetCustomAttribute<ParameterAttribute>() is not null)
            {
                parameters.Add(member);
            }
            else if (member.GetCustomAttribute<FlagAttribute>() is not null)
            {
                if (!writer.SkipFlags)
                {
                    flags.Add(member);
                }
            }
            else if (member.GetCustomAttribute<PropertyAttribute>() is not null)
            {
                properties.Add(member);
            }
        }

        VirtualParadiseCommand defaultInstance = (VirtualParadiseCommand)Activator.CreateInstance(command.GetType())!;

        if (parameters.Count > 0)
        {
            SerializeParameters(writer, command, defaultInstance, parameters, options);
        }

        if (flags.Count > 0)
        {
            SerializeFlags(writer, command, flags);
        }

        if (properties.Count > 0)
        {
            SerializeProperties(writer, command, defaultInstance, properties, options);
        }
    }

    private static void SerializeParameters(Utf8ActionWriter writer,
        VirtualParadiseCommand command,
        VirtualParadiseCommand defaultInstance,
        List<PropertyInfo> parameters,
        ActionSerializerOptions options)
    {
        for (var index = 0; index < parameters.Count; index++)
        {
            var parameter = parameters[index];
            ParameterAttribute? attribute = parameter.GetCustomAttribute<ParameterAttribute>();

            if (attribute is null)
            {
                throw new InvalidOperationException($"Parameter {parameter.Name} is missing a {nameof(ParameterAttribute)}.");
            }

            if (attribute.IsOptional && Equals(parameter.GetValue(command), parameter.GetValue(defaultInstance)))
            {
                continue;
            }

            WriteConvertedValue(writer, parameter, command, options);
        }
    }

    private static void SerializeFlags(Utf8ActionWriter writer, VirtualParadiseCommand command, List<PropertyInfo> flags)
    {
        for (var index = 0; index < flags.Count; index++)
        {
            PropertyInfo flag = flags[index];
            FlagAttribute? attribute = flag.GetCustomAttribute<FlagAttribute>();

            if (attribute is null)
            {
                throw new InvalidOperationException($"Flag {flag.Name} is missing a {nameof(FlagAttribute)}.");
            }

            if (flag.GetValue(command) is true)
            {
                writer.Write(attribute.Name);
            }
        }
    }

    private static void SerializeProperties(Utf8ActionWriter writer,
        VirtualParadiseCommand command,
        VirtualParadiseCommand defaultInstance,
        List<PropertyInfo> properties,
        ActionSerializerOptions options)
    {
        for (var index = 0; index < properties.Count; index++)
        {
            PropertyInfo property = properties[index];
            PropertyAttribute? attribute = property.GetCustomAttribute<PropertyAttribute>();

            if (attribute is null)
            {
                throw new InvalidOperationException($"Property {property.Name} is missing a {nameof(PropertyAttribute)}.");
            }

            if (attribute.IsOptional && Equals(property.GetValue(command), property.GetValue(defaultInstance)))
            {
                continue;
            }

            writer.WriteProperty(attribute.Name, GetConvertedValue(property, command, options));
        }
    }

    private static void WriteConvertedValue(Utf8ActionWriter writer,
        PropertyInfo parameter,
        VirtualParadiseCommand command,
        ActionSerializerOptions options)
    {
        ValueConverter? valueConverter;
        Type underlyingType = GetUnderlyingType(parameter);
        object? value = parameter.GetValue(command);
        object? defaultValue = CreateDefaultInstance(parameter);
        if ((value is null || Equals(value, defaultValue)) &&
            parameter.GetCustomAttribute<ParameterAttribute>()?.IsOptional != true)
        {
            return;
        }

        if (IsOptional(parameter) || IsNullableValueType(parameter))
        {
            value = value!.GetType().GetProperty("Value", PropertyBindingFlags)!.GetValue(value);
        }

        if (parameter.GetCustomAttribute<ValueConverterAttribute>() is { } valueConverterAttribute)
        {
            valueConverter = Activator.CreateInstance(valueConverterAttribute.ConverterType) as ValueConverter;

            if (valueConverter is not null)
            {
                valueConverter.Write(writer, underlyingType, value, options);
                return;
            }
        }

        foreach (Type valueConverterType in options.ValueConverterTypes)
        {
            valueConverter = Activator.CreateInstance(valueConverterType) as ValueConverter;

            if (valueConverter is null)
            {
                continue;
            }

            if (valueConverter.CanConvert(underlyingType))
            {
                valueConverter.Write(writer, underlyingType, value, options);
                return;
            }
        }

        writer.Write(value!.ToString());
    }

    private static string GetConvertedValue(PropertyInfo property,
        VirtualParadiseCommand command,
        ActionSerializerOptions options)
    {
        using var stream = new MemoryStream();
        var writer = new Utf8ActionWriter(stream);
        WriteConvertedValue(writer, property, command, options);
        return Encoding.GetString(stream.ToArray()).Trim();
    }
}
