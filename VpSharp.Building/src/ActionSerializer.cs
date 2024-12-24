using System.Reflection;
using System.Text;
using VpSharp.Building.Commands;
using VpSharp.Building.Extensions;
using VpSharp.Building.Triggers;

namespace VpSharp.Building;

/// <summary>
///     Represents a class that can serialize and deserialize action strings.
/// </summary>
public static partial class ActionSerializer
{
    private const BindingFlags PropertyBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    private static readonly Encoding Encoding = Encoding.UTF8;

    private static VirtualParadiseCommand? FindCommand(ReadOnlySpan<char> tokenValue, IEnumerable<Type> commandTypes)
    {
        foreach (Type type in commandTypes)
        {
            if (type.GetCommandName().AsSpan().Equals(tokenValue, StringComparison.OrdinalIgnoreCase))
            {
                return Activator.CreateInstance(type) as VirtualParadiseCommand;
            }
        }

        return null;
    }

    private static VirtualParadiseTrigger? FindTrigger(ReadOnlySpan<char> tokenValue, IEnumerable<Type> triggerTypes)
    {
        foreach (Type type in triggerTypes)
        {
            if (type.GetTriggerName().AsSpan().Equals(tokenValue, StringComparison.OrdinalIgnoreCase))
            {
                return Activator.CreateInstance(type) as VirtualParadiseTrigger;
            }
        }

        return null;
    }
}
