using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>opacity</c> command.
/// </summary>
[Command("opacity")]
public sealed class OpacityCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the opacity value.
    /// </summary>
    /// <value>The opacity value.</value>
    [Parameter(0)]
    public double Opacity { get; set; } = 1.0;
}
