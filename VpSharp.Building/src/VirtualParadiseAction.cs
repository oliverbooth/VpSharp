using VpSharp.Building.Triggers;

namespace VpSharp.Building;

/// <summary>
///     Represents an action.
/// </summary>
public sealed class VirtualParadiseAction
{
    private readonly Lazy<ActivateTrigger> _activateTrigger;
    private readonly Lazy<AdoneTrigger> _adoneTrigger;
    private readonly Lazy<BumpTrigger> _bumpTrigger;
    private readonly Lazy<BumpEndTrigger> _bumpEndTrigger;
    private readonly Lazy<CreateTrigger> _createTrigger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseAction" /> class.
    /// </summary>
    public VirtualParadiseAction()
    {
        _activateTrigger = new Lazy<ActivateTrigger>(LazyEvaluate<ActivateTrigger>());
        _adoneTrigger = new Lazy<AdoneTrigger>(LazyEvaluate<AdoneTrigger>());
        _bumpTrigger = new Lazy<BumpTrigger>(LazyEvaluate<BumpTrigger>());
        _bumpEndTrigger = new Lazy<BumpEndTrigger>(LazyEvaluate<BumpEndTrigger>());
        _createTrigger = new Lazy<CreateTrigger>(LazyEvaluate<CreateTrigger>());
    }

    /// <summary>
    ///     Gets the <c>activate</c> trigger.
    /// </summary>
    /// <value>The <see cref="ActivateTrigger" /> in this action.</value>
    public ActivateTrigger Activate
    {
        get => _activateTrigger.Value;
    }

    /// <summary>
    ///     Gets the <c>adone</c> trigger.
    /// </summary>
    /// <value>The <see cref="AdoneTrigger" /> in this action.</value>
    public AdoneTrigger Adone
    {
        get => _adoneTrigger.Value;
    }

    /// <summary>
    ///     Gets the <c>bump</c> trigger.
    /// </summary>
    /// <value>The <see cref="BumpTrigger" /> in this action.</value>
    public BumpTrigger Bump
    {
        get => _bumpTrigger.Value;
    }

    /// <summary>
    ///     Gets the <c>bumpend</c> trigger.
    /// </summary>
    /// <value>The <see cref="BumpEndTrigger" /> in this action.</value>
    public BumpEndTrigger BumpEnd
    {
        get => _bumpEndTrigger.Value;
    }

    /// <summary>
    ///     Gets the <c>create</c> trigger.
    /// </summary>
    /// <value>The <see cref="CreateTrigger" /> in this action.</value>
    public CreateTrigger Create
    {
        get => _createTrigger.Value;
    }

    /// <summary>
    ///     Gets the triggers in this action.
    /// </summary>
    /// <value>A read-only view of the triggers in this action.</value>
    public IReadOnlyList<VirtualParadiseTrigger> Triggers { get; internal set; } = [];

    private T LazyEvaluate<T>() where T : VirtualParadiseTrigger, new()
    {
        return Triggers.OfType<T>().FirstOrDefault() ?? new T();
    }
}
