using VpSharp.Building.Commands;
using VpSharp.Building.Serialization.ValueConverters;
using VpSharp.Building.Triggers;

namespace VpSharp.Building.Serialization;

/// <summary>
///     Allows customization of the behaviour of the serialization methods in <see cref="ActionSerializer" />.
/// </summary>
public sealed class ActionSerializerOptions
{
    private readonly List<Type> _commandTypes = [];
    private readonly List<Type> _triggerTypes = [];
    private readonly List<Type> _valueConverterTypes = [];

    /// <summary>
    ///     Default action serializer options.
    /// </summary>
    public static readonly ActionSerializerOptions Default = new()
    {
        CommandTypes = GetBuiltInCommands(),
        TriggerTypes = GetBuiltInTriggers(),
        ValueConverterTypes = GetBuiltInValueConverters()
    };

    /// <summary>
    ///     Gets the registered command types.
    /// </summary>
    /// <value>The registered command types.</value>
    public IReadOnlyCollection<Type> CommandTypes
    {
        get => _commandTypes.AsReadOnly();
        init => _commandTypes = [..value];
    }

    /// <summary>
    ///     Gets the registered trigger types.
    /// </summary>
    /// <value>The registered trigger types.</value>
    public IReadOnlyCollection<Type> TriggerTypes
    {
        get => _triggerTypes.AsReadOnly();
        init => _triggerTypes = [..value];
    }

    /// <summary>
    ///     Gets the registered value converter types.
    /// </summary>
    /// <value>The registered value converter types.</value>
    public IReadOnlyCollection<Type> ValueConverterTypes
    {
        get => _valueConverterTypes.AsReadOnly();
        init => _valueConverterTypes = [..value];
    }

    /// <summary>
    ///     Gets or initializes a value indicating whether the action should be written with indentation.
    /// </summary>
    /// <value><see langword="true" /> to write indented; otherwise, <see langword="false" />.</value>
    public bool WriteIndented { get; init; }

    internal void ValidateTypes()
    {
        foreach (Type? type in _commandTypes)
        {
            ValidateType<VirtualParadiseCommand>(type);
        }

        foreach (Type? type in _triggerTypes)
        {
            ValidateType<VirtualParadiseTrigger>(type);
        }

        foreach (Type? type in _valueConverterTypes)
        {
            ValidateType<ValueConverter>(type);
        }

        return;

        void ValidateType<T>(Type? type)
        {
            if (type is null)
            {
                throw new InvalidOperationException("Null type is invalid.");
            }

            if (type.IsAbstract || !type.IsSubclassOf(typeof(T)))
            {
                throw new InvalidOperationException($"Type '{type.FullName}' is not a valid {typeof(T).Name}.");
            }
        }
    }

    private static Type[] GetBuiltInCommands()
    {
        Type[] types = typeof(VirtualParadiseCommand).Assembly.GetTypes();
        return types.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(VirtualParadiseCommand))).ToArray();
    }

    private static Type[] GetBuiltInTriggers()
    {
        Type[] types = typeof(VirtualParadiseCommand).Assembly.GetTypes();
        return types.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(VirtualParadiseTrigger))).ToArray();
    }

    private static Type[] GetBuiltInValueConverters()
    {
        Type[] types = typeof(ValueConverter).Assembly.GetTypes();
        return types.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ValueConverter))).ToArray();
    }
}
