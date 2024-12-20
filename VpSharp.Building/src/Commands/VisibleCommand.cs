using VpSharp.Building.Annotations;
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

    /// <summary>
    ///     Gets or sets the target object name.
    /// </summary>
    /// <value>The target object name.</value>
    public string? Target { get; set; }

    /// <summary>
    ///     Gets or sets the radius value.
    /// </summary>
    /// <value>The radius value.</value>
    [Property("radius", IsOptional = true)]
    public double Radius { get; set; } = -1.0;
}
