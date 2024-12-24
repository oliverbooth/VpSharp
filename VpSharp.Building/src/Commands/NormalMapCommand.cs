using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>normalmap</c> command.
/// </summary>
[Command("normalmap")]
public sealed class NormalMapCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the normal map texture mask.
    /// </summary>
    /// <value>The normal map texture mask.</value>
    [Property("mask")]
    public string? Mask { get; set; }

    /// <summary>
    ///     Gets or sets the tag to which the normal map texture is applied.
    /// </summary>
    /// <value>The tag to which the normal map texture is applied.</value>
    [Property("tag")]
    public string? Tag { get; set; }

    /// <summary>
    ///     Gets or sets the normal map texture name.
    /// </summary>
    /// <value>The normal map texture name.</value>
    [Parameter(0)]
    public string Texture { get; set; } = string.Empty;
}
