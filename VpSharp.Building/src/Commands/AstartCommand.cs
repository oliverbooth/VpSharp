using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>astart</c> command.
/// </summary>
[Command("astart")]
public sealed class AstartCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets a value indicating whether the animation is looping.
    /// </summary>
    /// <value><see langword="true" /> if the animation is looping; otherwise, <see langword="false" />.</value>
    [Flag("looping")]
    public bool IsLooping { get; set; }

    /// <summary>
    ///     Gets or sets the name value.
    /// </summary>
    /// <value>The name value.</value>
    [Parameter(0)]
    public string Name { get; set; } = string.Empty;
}
