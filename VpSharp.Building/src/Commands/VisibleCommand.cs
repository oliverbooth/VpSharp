using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>visible</c> command.
/// </summary>
[Command("visible")]
public sealed class VisibleCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the visible flag value.
    /// </summary>
    /// <value>The visible flag value.</value>
    [Parameter(1, true)]
    public bool IsVisible { get; set; }

    /// <summary>
    ///     Gets the target name.
    /// </summary>
    /// <value>The target name.</value>
    [Parameter(0, IsOptional = true)]
    [Property("name", IsOptional = true)]
    public override string? TargetName { get; internal set; }
}
