using VpSharp.Building.Commands.Converters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>visible</c> command.
/// </summary>
[Command("visible", ConverterType = typeof(VisibleCommandConverter))]
public sealed class VisibleCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the visible flag value.
    /// </summary>
    /// <value>The visible flag value.</value>
    public bool IsVisible { get; set; } = true;
}
