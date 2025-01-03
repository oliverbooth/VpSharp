namespace VpSharp.Building.Triggers;

/// <summary>
///     Specifies the name of a trigger.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class TriggerAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TriggerAttribute" /> class.
    /// </summary>
    /// <param name="triggerName">The trigger name.</param>
    /// <exception cref="ArgumentNullException"><paramref name="triggerName" /> is <see langword="null" />.</exception>
    public TriggerAttribute(string triggerName)
    {
        TriggerName = triggerName ?? throw new ArgumentNullException(nameof(triggerName));
    }

    /// <summary>
    ///     Gets the trigger name.
    /// </summary>
    /// <value>The trigger name.</value>
    public string TriggerName { get; }
}
