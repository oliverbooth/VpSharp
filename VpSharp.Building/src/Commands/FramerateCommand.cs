using VpSharp.Building.Commands.Converters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>framerate</c> command.
/// </summary>
[Command("framerate", ConverterType = typeof(FramerateCommandConverter))]
public sealed class FramerateCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the framerate value.
    /// </summary>
    /// <value>The framerate value.</value>
    public int Framerate { get; set; } = 10;
}
