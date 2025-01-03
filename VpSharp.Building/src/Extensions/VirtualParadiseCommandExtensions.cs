using System.Collections.Concurrent;
using VpSharp.Building.Commands;
using X10D.Reflection;

namespace VpSharp.Building.Extensions;

/// <summary>
///     Extension methods for <see cref="VirtualParadiseCommand" />.
/// </summary>
public static class VirtualParadiseCommandExtensions
{
    private static readonly ConcurrentDictionary<Type, string> CommandNames = [];

    /// <summary>
    ///     Gets the name of the current command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>The command name.</returns>
    public static string GetCommandName(this VirtualParadiseCommand command)
    {
        return command.GetType().GetCommandName();
    }

    /// <summary>
    ///     Gets the name of the command type.
    /// </summary>
    /// <param name="type">The command type.</param>
    /// <returns>The command name.</returns>
    public static string GetCommandName(this Type type)
    {
        if (CommandNames.TryGetValue(type, out var name))
        {
            return name;
        }

        name = type.SelectFromCustomAttribute<CommandAttribute, string>(a => a.CommandName);

        if (!string.IsNullOrEmpty(name))
        {
            CommandNames[type] = name;
        }

        return name ?? string.Empty;
    }
}
