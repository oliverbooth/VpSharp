using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>solid</c> command.
/// </summary>
[Command("solid")]
public sealed class SolidCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the solid flag value.
    /// </summary>
    /// <value>The solid flag value.</value>
    [Parameter(1, true)]
    public bool IsSolid { get; set; }

    /// <summary>
    ///     Gets the target name.
    /// </summary>
    /// <value>The target name.</value>
    [Parameter(0, IsOptional = true)]
    [Property("name", IsOptional = true)]
    public override string? TargetName { get; internal set; }
}
