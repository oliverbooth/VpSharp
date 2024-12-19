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
        get => Triggers.OfType<ActivateTrigger>().FirstOrDefault() ?? new ActivateTrigger();
    }

    /// <summary>
    ///     Gets the <c>adone</c> trigger.
    /// </summary>
    /// <value>The <see cref="AdoneTrigger" /> in this action.</value>
    public AdoneTrigger Adone
    {
        get => Triggers.OfType<AdoneTrigger>().FirstOrDefault() ?? new AdoneTrigger();
    }

    /// <summary>
    ///     Gets the <c>bump</c> trigger.
    /// </summary>
    /// <value>The <see cref="BumpTrigger" /> in this action.</value>
    public BumpTrigger Bump
    {
        get => Triggers.OfType<BumpTrigger>().FirstOrDefault() ?? new BumpTrigger();
    }

    /// <summary>
    ///     Gets the <c>bumpend</c> trigger.
    /// </summary>
    /// <value>The <see cref="BumpEndTrigger" /> in this action.</value>
    public BumpEndTrigger BumpEnd
    {
        get => Triggers.OfType<BumpEndTrigger>().FirstOrDefault() ?? new BumpEndTrigger();
    }

    /// <summary>
    ///     Gets the <c>create</c> trigger.
    /// </summary>
    /// <value>The <see cref="CreateTrigger" /> in this action.</value>
    public CreateTrigger Create
    {
        get => Triggers.OfType<CreateTrigger>().FirstOrDefault() ?? new CreateTrigger();
    }

    /// <summary>
    ///     Gets the triggers in this action.
    /// </summary>
    /// <value>A read-only view of the triggers in this action.</value>
    public IReadOnlyList<VirtualParadiseTrigger> Triggers { get; internal set; } = [];
}
