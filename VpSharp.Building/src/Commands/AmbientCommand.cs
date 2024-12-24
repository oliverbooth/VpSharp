using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>ambient</c> command.
/// </summary>
[Command("ambient")]
public sealed class AmbientCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the ambient value.
    /// </summary>
    /// <value>The ambient value.</value>
    [Parameter(0)]
    public double Intensity { get; set; } = 1.0;

    /// <summary>
    ///     Gets or sets the tag to which the ambient intensity is applied.
    /// </summary>
    /// <value>The tag to which the ambient intensity is applied.</value>
    [Property("tag")]
    public string? Tag { get; set; }
}
