using System.Drawing;
using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>sign</c> command.
/// </summary>
[Command("sign")]
public sealed class SignCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the text alignment.
    /// </summary>
    /// <value>A <see cref="TextAlignment" /> value.</value>
    [Property("align")]
    public TextAlignment Alignment { get; set; } = TextAlignment.Center;

    /// <summary>
    ///     Gets or sets the background color.
    /// </summary>
    /// <value>A <see cref="Color" /> value representing the background color.</value>
    [Property("bcolor")]
    public Color BackColor { get; set; } = VpColors.DefaultSignBackColor;

    /// <summary>
    ///     Gets or sets the foreground color.
    /// </summary>
    /// <value>A <see cref="Color" /> value representing the foreground color.</value>
    [Property("color")]
    public Color ForeColor { get; set; } = Color.White;

    /// <summary>
    ///     Gets or sets the horizontal margin.
    /// </summary>
    /// <value>The horizontal margin.</value>
    [Property("hmargin")]
    public double HorizontalMargin { get; set; }

    /// <summary>
    ///     Gets or sets the overall margin.
    /// </summary>
    /// <value>The margin.</value>
    [Property("margin")]
    public double Margin { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether a shadow should be displayed.
    /// </summary>
    /// <value><see langword="true" /> if a shadow should be displayed; otherwise, <see langword="false" />.</value>
    [Flag("shadow")]
    public bool Shadow { get; set; }

    /// <summary>
    ///     Gets or sets the sign text.
    /// </summary>
    /// <value>The sign text.</value>
    [Parameter(0)]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the vertical margin.
    /// </summary>
    /// <value>The vertical margin.</value>
    [Property("vmargin")]
    public double VerticalMargin { get; set; }
}
