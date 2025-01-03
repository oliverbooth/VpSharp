using VpSharp.Building.Triggers;

namespace VpSharp.Building;

/// <summary>
///     Represents a mutable <see cref="VirtualParadiseAction" />.
/// </summary>
public sealed class VirtualParadiseActionBuilder
{
    private readonly List<VirtualParadiseTrigger> _triggers = [];

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseActionBuilder" /> class.
    /// </summary>
    public VirtualParadiseActionBuilder()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseActionBuilder" /> class.
    /// </summary>
    /// <param name="action">The action from which to copy the triggers and commands.</param>
    public VirtualParadiseActionBuilder(VirtualParadiseAction action)
    {
        _triggers.AddRange(action.Triggers);
    }

    /// <summary>
    ///     Adds a trigger to the action.
    /// </summary>
    /// <param name="trigger">The trigger to add.</param>
    /// <typeparam name="T">The type of trigger to add.</typeparam>
    /// <returns>The current <see cref="VirtualParadiseActionBuilder" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="trigger" /> is <see langword="null" />.</exception>
    public VirtualParadiseActionBuilder AddTrigger<T>(T trigger) where T : VirtualParadiseTrigger
    {
        if (trigger is null)
        {
            throw new ArgumentNullException(nameof(trigger));
        }

        _triggers.Add(trigger);
        return this;
    }

    /// <summary>
    ///     Adds a trigger to the action.
    /// </summary>
    /// <param name="action">A function that configures the trigger.</param>
    /// <typeparam name="T">The type of trigger to add.</typeparam>
    /// <returns>The current <see cref="VirtualParadiseActionBuilder" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
    public VirtualParadiseActionBuilder AddTrigger<T>(Action<VirtualParadiseTriggerBuilder<T>> action)
        where T : VirtualParadiseTrigger, new()
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var builder = new VirtualParadiseTriggerBuilder<T>();
        action(builder);
        _triggers.Add(builder.Build());
        return this;
    }

    /// <summary>
    ///     Builds the action.
    /// </summary>
    /// <returns>The built <see cref="VirtualParadiseAction" />.</returns>
    public VirtualParadiseAction Build()
    {
        return new VirtualParadiseAction { Triggers = _triggers.ToArray() };
    }

    /// <summary>
    ///     Implicitly converts this <see cref="VirtualParadiseActionBuilder" /> to a <see cref="VirtualParadiseAction" />.
    /// </summary>
    /// <param name="builder">The builder to convert.</param>
    /// <returns>The built <see cref="VirtualParadiseAction" />.</returns>
    public static implicit operator VirtualParadiseAction(VirtualParadiseActionBuilder builder)
    {
        return builder.Build();
    }
}
