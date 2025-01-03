using VpSharp.Building.Commands;
using VpSharp.Building.Triggers;

namespace VpSharp.Building;

/// <summary>
///     Represents a mutable <see cref="VirtualParadiseTrigger" />.
/// </summary>
public sealed class VirtualParadiseTriggerBuilder<T> where T : VirtualParadiseTrigger, new()
{
    private readonly List<VirtualParadiseCommand> _commands = [];

    internal VirtualParadiseTriggerBuilder()
    {
    }

    /// <summary>
    ///     Adds a command to the trigger.
    /// </summary>
    /// <param name="command">The command to add.</param>
    /// <typeparam name="TCommand">The type of command to add.</typeparam>
    /// <returns>The current <see cref="VirtualParadiseTriggerBuilder{T}" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="command" /> is <see langword="null" />.</exception>
    public VirtualParadiseTriggerBuilder<T> AddCommand<TCommand>(TCommand command)
        where TCommand : VirtualParadiseCommand
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        _commands.Add(command);
        return this;
    }

    /// <summary>
    ///     Adds a command to the trigger.
    /// </summary>
    /// <param name="builder">A function that configures the command.</param>
    /// <typeparam name="TCommand">The type of command to add.</typeparam>
    /// <returns>The current <see cref="VirtualParadiseTriggerBuilder{T}" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public VirtualParadiseTriggerBuilder<T> AddCommand<TCommand>(Action<TCommand> builder)
        where TCommand : VirtualParadiseCommand, new()
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var command = new TCommand();
        builder(command);

        _commands.Add(command);
        return this;
    }

    internal VirtualParadiseTrigger Build()
    {
        return new T { Commands = _commands.ToArray() };
    }
}
