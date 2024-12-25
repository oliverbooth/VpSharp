using Optional;
using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>diffuse</c> command.
/// </summary>
[Command("diffuse")]
public sealed class DiffuseCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the diffuse value.
    /// </summary>
    /// <value>The diffuse value.</value>
    [Parameter(0)]
    public double Intensity { get; set; } = 1.0;

    /// <summary>
    ///     Gets or sets the tag to which the diffuse intensity is applied.
    /// </summary>
    /// <value>The tag to which the diffuse intensity is applied.</value>
    [Property("tag")]
    public Option<string> Tag { get; set; }
}
