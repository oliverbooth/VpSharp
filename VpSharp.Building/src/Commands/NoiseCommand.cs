using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>noise</c> command.
/// </summary>
[Command("noise")]
public sealed class NoiseCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the file name or URL of the noise.
    /// </summary>
    /// <value>The file name or URL of the noise.</value>
    [Parameter(0)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a value indicating whether this noise loops.
    /// </summary>
    /// <value><see langword="true" /> if this noise loops; otherwise, <see langword="false" />.</value>
    [Flag("loop")]
    public bool IsLooping { get; set; }

    /// <summary>
    ///     Gets or sets the volume of this noise, between 0.0 and 1.0.
    /// </summary>
    /// <value>The volume of this noise, between 0.0 and 1.0.</value>
    [Property("volume")]
    public double Volume { get; set; } = 1.0;
}
