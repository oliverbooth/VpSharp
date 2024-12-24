using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>framerate</c> command.
/// </summary>
[Command("framerate")]
public sealed class FramerateCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the framerate value.
    /// </summary>
    /// <value>The framerate value.</value>
    [Parameter(0)]
    public int Framerate { get; set; } = 10;
}
