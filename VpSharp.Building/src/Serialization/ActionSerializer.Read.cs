using System.Reflection;
using Cysharp.Text;
using VpSharp.Building.Annotations;
using VpSharp.Building.Commands;
using VpSharp.Building.Serialization.ValueConverters;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Serialization;

public static partial class ActionSerializer
{
    /// <summary>
    ///     Deserializes an action from the specified span of UTF-8 encoded bytes.
    /// </summary>
    /// <param name="utf8ActionStream">The stream containing UTF-8 encoded bytes to read.</param>
    /// <param name="options">An <see cref="ActionSerializerOptions" /> object that specifies deserialization behaviour.</param>
    /// <returns>The deserialized action.</returns>
    /// <exception cref="InvalidOperationException">An invalid command or trigger type was supplied.</exception>
    public static VirtualParadiseAction Deserialize(Stream utf8ActionStream, ActionSerializerOptions? options = null)
    {
        if (utf8ActionStream is null)
        {
            throw new ArgumentNullException(nameof(utf8ActionStream));
        }

        options ??= ActionSerializerOptions.Default;
        options.ValidateTypes();

        try
        {
            if (utf8ActionStream.Length >= int.MaxValue)
            {
                throw new InvalidOperationException("The action is too large to be deserialized.");
            }

            int byteCount = (int)utf8ActionStream.Length;
            Span<byte> utf8Action = stackalloc byte[byteCount];
            utf8ActionStream.ReadExactly(utf8Action);

            return Deserialize(utf8Action, options);
        }
        catch (NotSupportedException)
        {
            using var stream = new MemoryStream();
            utf8ActionStream.CopyTo(stream);

            return Deserialize(stream.ToArray(), options);
        }
    }

    /// <summary>
    ///     Deserializes an action from the specified span of UTF-8 encoded bytes.
    /// </summary>
    /// <param name="utf8Action">The span of UTF-8 encoded bytes to read.</param>
    /// <param name="options">An <see cref="ActionSerializerOptions" /> object that specifies deserialization behaviour.</param>
    /// <returns>The deserialized action.</returns>
    /// <exception cref="InvalidOperationException">An invalid command or trigger type was supplied.</exception>
    public static VirtualParadiseAction Deserialize(ReadOnlySpan<byte> utf8Action, ActionSerializerOptions? options = null)
    {
        options ??= ActionSerializerOptions.Default;
        options.ValidateTypes();

        int charCount = Encoding.GetCharCount(utf8Action);
        Span<char> action = stackalloc char[charCount];
        Encoding.GetChars(utf8Action, action);

        return Deserialize(action, options);
    }

    /// <summary>
    ///     Deserializes an action from the specified span of characters.
    /// </summary>
    /// <param name="source">The span of characters to read.</param>
    /// <param name="options">An <see cref="ActionSerializerOptions" /> object that specifies deserialization behaviour.</param>
    /// <returns>The deserialized action.</returns>
    /// <exception cref="InvalidOperationException">An invalid command or trigger type was supplied.</exception>
    public static VirtualParadiseAction Deserialize(ReadOnlySpan<char> source, ActionSerializerOptions? options = null)
    {
        options ??= ActionSerializerOptions.Default;
        options.ValidateTypes();

        using Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();
        var triggers = new List<VirtualParadiseTrigger>();
        bool isQuoted = false;

        for (var index = 0; index < source.Length; index++)
        {
            char current = source[index];
            switch (current)
            {
                case '"':
                    isQuoted = !isQuoted;
                    goto default;

                case ';' when !isQuoted:
                    HandleBuffer();
                    break;

                default:
                    builder.Append(current);
                    break;
            }

            if (index == source.Length - 1)
            {
                HandleBuffer();
            }
        }

        return new VirtualParadiseAction { Triggers = triggers };

        void HandleBuffer()
        {
            VirtualParadiseTrigger? trigger = DeserializeTrigger(builder.AsSpan(), options);
            if (trigger is not null)
            {
                triggers.Add(trigger);
            }

            builder.Clear();
        }
    }

    private static VirtualParadiseTrigger? DeserializeTrigger(ReadOnlySpan<char> source, ActionSerializerOptions options)
    {
        Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();
        var commands = new List<VirtualParadiseCommand>();
        bool isQuoted = false;
        bool isTriggerName = true;
        VirtualParadiseTrigger? trigger = null;

        foreach (char current in source)
        {
            HandleCharacter(current);
        }

        if (isTriggerName)
        {
            trigger = FindTrigger(builder.AsSpan(), options.TriggerTypes);
            builder.Clear();
        }
        else
        {
            HandleTriggerBuffer(ref builder, options, commands);
        }

        if (trigger is not null)
        {
            trigger.Commands = commands.AsReadOnly();
        }

        builder.Dispose();
        return trigger;

        void HandleCharacter(char character)
        {
            switch (character)
            {
                case ' ' when isTriggerName:
                    isTriggerName = false;
                    trigger = FindTrigger(builder.AsSpan(), options.TriggerTypes);
                    builder.Clear();
                    break;

                case '"':
                    isQuoted = !isQuoted;
                    goto default;

                case ',' when !isQuoted:
                    HandleTriggerBuffer(ref builder, options, commands);
                    break;

                default:
                    builder.Append(character);
                    break;
            }
        }
    }

    private static void HandleTriggerBuffer(ref Utf16ValueStringBuilder builder, ActionSerializerOptions options,
        List<VirtualParadiseCommand> commands)
    {
        VirtualParadiseCommand? command = DeserializeCommand(builder.AsSpan(), options);
        if (command is not null)
        {
            commands.Add(command);
        }

        builder.Clear();
    }

    private static VirtualParadiseCommand? DeserializeCommand(ReadOnlySpan<char> source, ActionSerializerOptions options)
    {
        Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();
        VirtualParadiseCommand? command = ParseCommand(source, options, ref builder);
        builder.Dispose();

        if (command is null)
        {
            return null;
        }

        if (command.GetType().GetCustomAttribute<CommandAttribute>()?.ConverterType is { } converterType)
        {
            DeserializeWithCommandConverter(command, converterType, options);
        }
        else
        {
            ExtractArguments(command, options);
        }

        ExtractProperties(command, options);
        ExtractFlags(command);
        return command;
    }

    private static void ExtractFlags(VirtualParadiseCommand command)
    {
        if (command.RawArguments.Count == 0)
        {
            return;
        }

        PropertyInfo[] members = command.GetType().GetProperties(PropertyBindingFlags);
        PropertyInfo[] flags = members.Where(m => m.GetCustomAttribute<FlagAttribute>() is not null).ToArray();

        foreach (PropertyInfo flag in flags)
        {
            if (flag.PropertyType != typeof(bool))
            {
                continue;
            }

            FlagAttribute attribute = flag.GetCustomAttribute<FlagAttribute>()!;
            if (!command.RawArguments.Contains(attribute.Name, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            flag.SetValue(command, OverlayValue(flag, true));
        }
    }

    private static void DeserializeWithCommandConverter(VirtualParadiseCommand command,
        Type converterType,
        ActionSerializerOptions options)
    {
        IList<string> arguments = command.RawArguments;
        IDictionary<string, string> properties = command.RawProperties;
        CommandConverter converter = (CommandConverter)Activator.CreateInstance(converterType)!;
        Utf16ValueStringBuilder builder = ZString.CreateStringBuilder();

        foreach (string argument in arguments)
        {
            builder.Append(argument);
            builder.Append(' ');
        }

        foreach (KeyValuePair<string, string> property in properties)
        {
            builder.Append(property.Key);
            builder.Append('=');
            builder.Append(property.Value);
            builder.Append(' ');
        }

        ReadOnlySpan<char> chars = builder.AsSpan().TrimEnd();
        int byteCount = Encoding.GetByteCount(chars);
        Span<byte> bytes = stackalloc byte[byteCount];
        Encoding.GetBytes(chars, bytes);

        var reader = new Utf8ActionReader(bytes);
        converter.Read(ref reader, command.GetType(), command, options);
    }

    private static void ExtractArguments(VirtualParadiseCommand command, ActionSerializerOptions options)
    {
        if (command.RawArguments.Count == 0)
        {
            return;
        }

        PropertyInfo[] members = command.GetType().GetProperties(PropertyBindingFlags);
        PropertyInfo[] parameters = members.Where(m => m.GetCustomAttribute<ParameterAttribute>() is not null)
            .OrderBy(m => m.GetCustomAttribute<ParameterAttribute>()!.Order)
            .ToArray();

        int maxBytes = command.RawArguments.Max(a => Encoding.GetByteCount(a));
        Span<byte> buffer = stackalloc byte[maxBytes];

        for (var index = 0; index < parameters.Length; index++)
        {
            PropertyInfo parameter = parameters[index];
            string? argument = index < command.RawArguments.Count ? command.RawArguments[index] : null;
            if (argument is null)
            {
                continue;
            }

            int byteCount = Encoding.GetByteCount(argument);
            Span<byte> bytes = buffer[..byteCount];
            Encoding.GetBytes(argument, bytes);

            var reader = new Utf8ActionReader(bytes);
            SetParameterValue(command, parameter, ref reader, argument, options);
        }
    }

    private static void SetParameterValue(VirtualParadiseCommand command,
        PropertyInfo parameter,
        ref Utf8ActionReader reader,
        string argument,
        ActionSerializerOptions options)
    {
        Type underlyingType = GetUnderlyingType(parameter);

        if (GetValueConverterType(parameter) is { } type)
        {
            ValueConverter converter = (ValueConverter)Activator.CreateInstance(type)!;
            object? value = converter.Read(ref reader, underlyingType, out bool success, options);

            if (success)
            {
                parameter.SetValue(command, OverlayValue(parameter, value));
            }
        }
        else if (underlyingType.IsEnum)
        {
            if (Enum.TryParse(underlyingType, argument, true, out object? value))
            {
                parameter.SetValue(command, OverlayValue(parameter, value));
            }
        }
        else
        {
            object value = Convert.ChangeType(argument, underlyingType);
            parameter.SetValue(command, OverlayValue(parameter, value));
        }
    }

    private static void ExtractProperties(VirtualParadiseCommand command, ActionSerializerOptions options)
    {
        if (command.RawProperties.Count == 0)
        {
            return;
        }

        PropertyInfo[] members = command.GetType().GetProperties(PropertyBindingFlags);
        PropertyInfo[] properties = members.Where(m => m.GetCustomAttribute<PropertyAttribute>() is not null).ToArray();

        int maxBytes = command.RawProperties.Max(a => Encoding.GetByteCount(a.Value));
        Span<byte> buffer = stackalloc byte[maxBytes];

        foreach (PropertyInfo property in properties)
        {
            PropertyAttribute attribute = property.GetCustomAttribute<PropertyAttribute>()!;
            if (!command.RawProperties.TryGetValue(attribute.Name, out string? rawValue))
            {
                continue;
            }

            int byteCount = Encoding.GetByteCount(rawValue);
            Span<byte> bytes = buffer[..byteCount];
            Encoding.GetBytes(rawValue, bytes);

            var reader = new Utf8ActionReader(bytes);

            SetPropertyValue(command, property, ref reader, rawValue, options);
        }
    }

    private static void SetPropertyValue(VirtualParadiseCommand command,
        PropertyInfo property,
        ref Utf8ActionReader reader,
        string rawValue,
        ActionSerializerOptions options)
    {
        Type underlyingType = GetUnderlyingType(property);
        if (GetValueConverterType(property) is { } type)
        {
            ValueConverter converter = (ValueConverter)Activator.CreateInstance(type)!;
            object? value = converter.Read(ref reader, underlyingType, out bool success, options);

            if (success)
            {
                property.SetValue(command, OverlayValue(property, value));
            }
        }
        else if (underlyingType.IsEnum)
        {
            if (Enum.TryParse(underlyingType, rawValue, true, out object? value))
            {
                property.SetValue(command, OverlayValue(property, value));
            }
        }
        else
        {
            object value = Convert.ChangeType(rawValue, underlyingType);
            property.SetValue(command, OverlayValue(property, value));
        }
    }

    private static Type? GetValueConverterType(PropertyInfo member)
    {
        if (member.GetCustomAttribute<ValueConverterAttribute>() is { } attribute)
        {
            return attribute.ConverterType;
        }

        return typeof(ValueConverter<>).Assembly.GetTypes().FirstOrDefault(t =>
            !t.IsAbstract && t.IsSubclassOf(typeof(ValueConverter<>).MakeGenericType(GetUnderlyingType(member))));
    }

    private static VirtualParadiseCommand? ParseCommand(ReadOnlySpan<char> source,
        ActionSerializerOptions options,
        ref Utf16ValueStringBuilder builder)
    {
        VirtualParadiseCommand? command = null;
        bool isQuoted = false;
        bool isCommandName = true;

        for (var index = 0; index < source.Length; index++)
        {
            HandleCharacter(source[index], ref builder);

            if (index != source.Length - 1)
            {
                continue;
            }

            if (isCommandName)
            {
                isCommandName = false;
                command = FindCommand(builder.AsSpan(), options.CommandTypes);
                builder.Clear();
            }
            else
            {
                HandleCommandBuffer(command, ref builder);
            }
        }

        return command;

        void HandleCharacter(char character, ref Utf16ValueStringBuilder builder)
        {
            switch (character)
            {
                case ' ' when isCommandName:
                    isCommandName = false;
                    command = FindCommand(builder.AsSpan(), options.CommandTypes);
                    builder.Clear();
                    break;

                case '"':
                    isQuoted = !isQuoted;
                    goto default;

                case ' ' when !isQuoted:
                    HandleCommandBuffer(command, ref builder);
                    break;

                default:
                    builder.Append(character);
                    break;
            }
        }
    }

    private static void HandleCommandBuffer(VirtualParadiseCommand? command, ref Utf16ValueStringBuilder builder)
    {
        if (command is null)
        {
            return;
        }

        ReadOnlySpan<char> span = builder.AsSpan();
        bool isQuoted = false;
        bool isProperty = false;
        int equalsIndex = -1;

        for (var index = 0; index < span.Length; index++)
        {
            var current = span[index];
            switch (current)
            {
                case '"':
                    isQuoted = !isQuoted;
                    break;

                case '=' when !isQuoted:
                    isProperty = true;
                    equalsIndex = index;
                    break;
            }
        }

        if (isProperty)
        {
            ReadOnlySpan<char> name = span[..equalsIndex];
            ReadOnlySpan<char> value = span[(equalsIndex + 1)..];

            command.RawProperties[name.ToString()] = value.ToString();
        }
        else
        {
            command.RawArguments.Add(span.ToString());
        }

        builder.Clear();
    }
}
