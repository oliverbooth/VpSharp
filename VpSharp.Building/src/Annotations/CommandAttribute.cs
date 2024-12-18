namespace VpSharp.Building.Annotations;

/// <summary>
///     Specifies the name of a command.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CommandAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandAttribute" /> class.
    /// </summary>
    /// <param name="commandName">The command name.</param>
    /// <exception cref="ArgumentNullException"><paramref name="commandName" /> is <see langword="null" />.</exception>
    public CommandAttribute(string commandName)
    {
        CommandName = commandName ?? throw new ArgumentNullException(nameof(commandName));
    }

    /// <summary>
    ///     Gets the command name.
    /// </summary>
    /// <value>The command name.</value>
    public string CommandName { get; }
}
