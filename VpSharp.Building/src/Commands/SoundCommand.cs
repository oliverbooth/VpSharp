using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>sound</c> command.
/// </summary>
[Command("sound")]
public sealed class SoundCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the file name or URL of the sound effect.
    /// </summary>
    /// <value>The file name or URL of the sound effect.</value>
    [Parameter(0)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the name of the object that plays the left audio channel.
    /// </summary>
    /// <value>The name of the object that plays the left audio channel.</value>
    [Property("leftspk")]
    public string? LeftSpeaker { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this sound doesn't loop.
    /// </summary>
    /// <value><see langword="true" /> if this sound doesn't loop; otherwise, <see langword="false" />.</value>
    [Flag("noloop")]
    public bool IsNonLooping { get; set; }

    /// <summary>
    ///     Gets or sets the radius that this sound can be heard.
    /// </summary>
    /// <value>The radius that this sound can be heard.</value>
    [Property("radius")]
    public double Radius { get; set; } = 50.0;

    /// <summary>
    ///     Gets or sets the name of the object that plays the right audio channel.
    /// </summary>
    /// <value>The name of the object that plays the right audio channel.</value>
    [Property("rightspk")]
    public string? RightSpeaker { get; set; }

    /// <summary>
    ///     Gets or sets the volume of this sound, between 0.0 and 1.0.
    /// </summary>
    /// <value>The volume of this sound, between 0.0 and 1.0.</value>
    [Property("volume")]
    public double Volume { get; set; } = 1.0;
}
