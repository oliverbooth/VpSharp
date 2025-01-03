using VpSharp.Building.Annotations;
using VpSharp.Building.Serialization.CommandConverters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>solid</c> command.
/// </summary>
[Command("solid", ConverterType = typeof(SolidCommandConverter))]
public sealed class SolidCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the solid flag value.
    /// </summary>
    /// <value>The solid flag value.</value>
    [Parameter(1)]
    public bool IsSolid { get; set; } = true;

    /// <summary>
    ///     Gets or sets the target object name.
    /// </summary>
    /// <value>The target object name.</value>
    [Parameter(0, IsOptional = true)]
    public string? Target { get; set; }
}
