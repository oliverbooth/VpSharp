using System.Collections.ObjectModel;
using System.Reflection;

namespace VpSharp.Commands;

/// <summary>
///     Represents a registered command.
/// </summary>
public sealed class Command
{
    internal Command(string name, IReadOnlyList<string> aliases, MethodInfo method, CommandModule module)
    {
        Name = name;
        Aliases = aliases;
        Method = method;
        Module = module;
        Parameters = method.GetParameters()[1..];
    }

    /// <summary>
    ///     Gets the aliases for this command.
    /// </summary>
    /// <value>The aliases.</value>
    public IReadOnlyList<string> Aliases { get; }

    /// <summary>
    ///     Gets the name of this command.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; }

    internal MethodInfo Method { get; }

    internal CommandModule Module { get; }

    internal ParameterInfo[] Parameters { get; }
}
