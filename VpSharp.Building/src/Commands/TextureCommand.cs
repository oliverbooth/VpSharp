using Optional;
using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>texture</c> command.
/// </summary>
[Command("texture")]
public sealed class TextureCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the texture mask.
    /// </summary>
    /// <value>The texture mask.</value>
    [Property("mask")]
    public Option<string> Mask { get; set; }

    /// <summary>
    ///     Gets or sets the tag to which the texture is applied.
    /// </summary>
    /// <value>The tag to which the texture is applied.</value>
    [Property("tag")]
    public Option<string> Tag { get; set; }

    /// <summary>
    ///     Gets or sets the texture name.
    /// </summary>
    /// <value>The texture name.</value>
    [Parameter(0)]
    public string Texture { get; set; } = string.Empty;
}
