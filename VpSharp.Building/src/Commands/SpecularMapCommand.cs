using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>specularmap</c> command.
/// </summary>
[Command("specularmap")]
public sealed class SpecularMapCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the specular map texture mask.
    /// </summary>
    /// <value>The specular map texture mask.</value>
    [Property("mask")]
    public string? Mask { get; set; }

    /// <summary>
    ///     Gets or sets the tag to which the specular map texture is applied.
    /// </summary>
    /// <value>The tag to which the specular map texture is applied.</value>
    [Property("tag")]
    public string? Tag { get; set; }

    /// <summary>
    ///     Gets or sets the specular map texture name.
    /// </summary>
    /// <value>The specular map texture name.</value>
    [Parameter(0)]
    public string Texture { get; set; } = string.Empty;
}
