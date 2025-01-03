using VpSharp.Building.Triggers;

namespace VpSharp.Building;

/// <summary>
///     Represents an action.
/// </summary>
public sealed class VirtualParadiseAction
{
    /// <summary>
    ///     Gets the <c>activate</c> trigger.
    /// </summary>
    /// <value>The <see cref="ActivateTrigger" /> in this action.</value>
    public ActivateTrigger Activate
    {
        get => new() { Commands = Triggers.OfType<ActivateTrigger>().SelectMany(t => t.Commands).ToArray() };
    }

    /// <summary>
    ///     Gets the <c>adone</c> trigger.
    /// </summary>
    /// <value>The <see cref="AdoneTrigger" /> in this action.</value>
    public AdoneTrigger Adone
    {
        get => new() { Commands = Triggers.OfType<AdoneTrigger>().SelectMany(t => t.Commands).ToArray() };
    }

    /// <summary>
    ///     Gets the <c>bump</c> trigger.
    /// </summary>
    /// <value>The <see cref="BumpTrigger" /> in this action.</value>
    public BumpTrigger Bump
    {
        get => new() { Commands = Triggers.OfType<BumpTrigger>().SelectMany(t => t.Commands).ToArray() };
    }

    /// <summary>
    ///     Gets the <c>bumpend</c> trigger.
    /// </summary>
    /// <value>The <see cref="BumpEndTrigger" /> in this action.</value>
    public BumpEndTrigger BumpEnd
    {
        get => new() { Commands = Triggers.OfType<BumpEndTrigger>().SelectMany(t => t.Commands).ToArray() };
    }

    /// <summary>
    ///     Gets the <c>create</c> trigger.
    /// </summary>
    /// <value>The <see cref="CreateTrigger" /> in this action.</value>
    public CreateTrigger Create
    {
        get => new() { Commands = Triggers.OfType<CreateTrigger>().SelectMany(t => t.Commands).ToArray() };
    }

    /// <summary>
    ///     Gets the triggers in this action.
    /// </summary>
    /// <value>A read-only view of the triggers in this action.</value>
    public IReadOnlyList<VirtualParadiseTrigger> Triggers { get; internal set; } = [];
}
