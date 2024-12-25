using Optional;
using VpSharp.Building.Annotations;
using VpSharp.Building.Commands.Converters;
using VpSharp.Building.ValueConverters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>animate</c> command.
/// </summary>
[Command("animate", ConverterType = typeof(AnimateCommandConverter))]
public sealed class AnimateCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the animation name.
    /// </summary>
    /// <value>The animation name.</value>
    [Parameter(1)]
    public string Animation { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the frame count.
    /// </summary>
    /// <value>The frame count.</value>
    [Parameter(3)]
    public int FrameCount { get; set; }

    /// <summary>
    ///     Gets or sets the delay between frames.
    /// </summary>
    /// <value>The frame delay.</value>
    [Parameter(4)]
    [ValueConverter(typeof(TimeSpanToMillisecondsValueConverter))]
    public TimeSpan FrameDelay { get; set; }

    /// <summary>
    ///     Gets or sets the frame list.
    /// </summary>
    /// <value>The frame list.</value>
    public IReadOnlyList<int> FrameList { get; set; } = [];

    /// <summary>
    ///     Gets or sets the image count.
    /// </summary>
    /// <value>The image count.</value>
    [Parameter(2)]
    public int ImageCount { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this animation is masked.
    /// </summary>
    /// <value><see langword="true" /> if this animation is masked; otherwise, <see langword="false" />.</value>
    [Flag("mask")]
    public bool IsMask { get; set; }

    /// <summary>
    ///     Gets or sets the target object to be animated.
    /// </summary>
    /// <value>The object name.</value>
    [Parameter(0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the tag to which the animation will be applied.
    /// </summary>
    /// <value>The tag.</value>
    [Property("tag")]
    public Option<string> Tag { get; set; }
}
