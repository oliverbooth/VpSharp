using VpSharp.Building.Annotations;
using VpSharp.Building.ValueConverters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>web</c> command.
/// </summary>
[Command("web")]
public sealed class WebCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the frame height.
    /// </summary>
    /// <value>The frame height.</value>
    [Property("sh")]
    public double FrameHeight { get; set; } = 512.0;

    /// <summary>
    ///     Gets or sets the frame width.
    /// </summary>
    /// <value>The frame width.</value>
    [Property("sw")]
    public double FrameWidth { get; set; } = 512.0;

    /// <summary>
    ///     Gets or sets a value indicating whether the frame should capture keystrokes.
    /// </summary>
    /// <value><see langword="true" /> if the frame should capture keystrokes; otherwise, <see langword="false" />.</value>
    [Property("keys")]
    [ValueConverter(typeof(YesNoBooleanValueConverter))]
    public bool Keys { get; set; }

    /// <summary>
    ///     Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    [Property("url", IsOptional = false)]
    public string Url { get; set; } = string.Empty;
}
