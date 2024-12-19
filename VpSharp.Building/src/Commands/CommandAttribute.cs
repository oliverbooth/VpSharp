namespace VpSharp.Building.Commands;

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

    /// <summary>
    ///     Gets or initializes the type of the converter for this command.
    /// </summary>
    /// <value>The type of the converter for this command.</value>
    public Type? ConverterType { get; init; }
}
