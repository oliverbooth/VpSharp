using System.Collections.Concurrent;
using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;
using X10D.Reflection;

namespace VpSharp.Building.Extensions;

/// <summary>
///     Extension methods for <see cref="VirtualParadiseCommand" />.
/// </summary>
public static class VirtualParadiseTriggerExtensions
{
    private static readonly ConcurrentDictionary<Type, string> TriggerNames = [];

    /// <summary>
    ///     Gets the name of the current trigger.
    /// </summary>
    /// <param name="trigger">The trigger.</param>
    /// <returns>The trigger name.</returns>
    public static string GetTriggerName(this VirtualParadiseTrigger trigger)
    {
        return trigger.GetType().GetTriggerName();
    }

    /// <summary>
    ///     Gets the name of the trigger type.
    /// </summary>
    /// <param name="type">The trigger type.</param>
    /// <returns>The trigger name.</returns>
    public static string GetTriggerName(this Type type)
    {
        if (TriggerNames.TryGetValue(type, out var name))
        {
            return name;
        }

        name = type.SelectFromCustomAttribute<TriggerAttribute, string>(a => a.TriggerName);

        if (!string.IsNullOrEmpty(name))
        {
            TriggerNames[type] = name;
        }

        return name ?? string.Empty;
    }
}
