using VpSharp.Building.Annotations;
using VpSharp.Building.ValueConverters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>picture</c> command.
/// </summary>
[Command("picture")]
public sealed class PictureCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the picture name.
    /// </summary>
    /// <value>The picture name.</value>
    [Parameter(0)]
    public string Picture { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the tag to which the texture is applied.
    /// </summary>
    /// <value>The tag to which the texture is applied.</value>
    [Property("tag")]
    public string? Tag { get; set; }

    /// <summary>
    ///     Gets or sets the time after which the texture is updated.
    /// </summary>
    /// <value>The time after which the texture is updated.</value>
    [Property("update")]
    [ValueConverter(typeof(TimeSpanToSecondsValueConverter))]
    public TimeSpan Update { get; set; }
}
