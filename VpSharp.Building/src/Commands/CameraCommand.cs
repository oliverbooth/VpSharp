using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>camera</c> command.
/// </summary>
[Command("camera")]
public sealed class CameraCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the location value.
    /// </summary>
    /// <value>The location value.</value>
    [Property("location")]
    public string? Location { get; set; }

    /// <summary>
    ///     Gets or sets the target value.
    /// </summary>
    /// <value>The target value.</value>
    [Property("target")]
    public string? Target { get; set; }
}
