namespace VpSharp.Commands.Attributes;

/// <summary>
///     Defines the name of a command.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CommandAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandAttribute" /> class.
    /// </summary>
    /// <param name="name">The command name.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException"><paramref name="name" /> is empty, or consists of only whitespace.</exception>
    public CommandAttribute(string name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty");
        }

        Name = name;
    }

    /// <summary>
    ///     Gets the command name.
    /// </summary>
    /// <value>The command name.</value>
    public string Name { get; }
}
