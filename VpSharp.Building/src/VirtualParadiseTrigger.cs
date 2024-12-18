namespace VpSharp.Building;

/// <summary>
///     Represents a trigger.
/// </summary>
public abstract class VirtualParadiseTrigger
{
    /// <summary>
    ///     Gets the commands in this trigger.
    /// </summary>
    /// <value>A read-only view of the commands in this trigger.</value>
    public IReadOnlyList<VirtualParadiseCommand> Commands { get; internal set; } = [];
}
