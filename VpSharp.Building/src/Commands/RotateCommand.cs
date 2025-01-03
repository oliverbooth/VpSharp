using Optional;
using VpSharp.Building.Annotations;
using VpSharp.Building.Serialization.CommandConverters;
using VpSharp.Building.Serialization.ValueConverters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>rotate</c> command.
/// </summary>
[Command("rotate", ConverterType = typeof(RotateCommandConverter))]
public sealed class RotateCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the scale factor.
    /// </summary>
    /// <value>The scale factor.</value>
    public Vector3d Rotation { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this rotation is along the object axis (as opposed to world axis).
    /// </summary>
    /// <value><see langword="true" /> if this rotation is along the object axis; otherwise, <see langword="false" />.</value>
    [Flag("ltm")]
    public bool IsLocalAxis { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this rotation loops.
    /// </summary>
    /// <value><see langword="true" /> if this rotation loops; otherwise, <see langword="false" />.</value>
    [Flag("loop")]
    public bool IsLooping { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this rotation is smooth.
    /// </summary>
    /// <value><see langword="true" /> if this rotation is smooth; otherwise, <see langword="false" />.</value>
    [Flag("smooth")]
    public bool IsSmooth { get; set; }

    /// <summary>
    ///     Gets or sets the offset to apply to universe time when synchronizing.
    /// </summary>
    /// <value>The offset.</value>
    [Property("offset")]
    [ValueConverter(typeof(TimeSpanToSecondsValueConverter))]
    public TimeSpan Offset { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this rotation resets after completing half a cycle.
    /// </summary>
    /// <value><see langword="true" /> if this rotation resets; otherwise, <see langword="false" />.</value>
    [Flag("reset")]
    public bool ShouldReset { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this rotation syncs.
    /// </summary>
    /// <value><see langword="true" /> if this rotation syncs; otherwise, <see langword="false" />.</value>
    [Flag("sync")]
    public bool ShouldSync { get; set; }

    /// <summary>
    ///     Gets or sets the duration of half of a cycle.
    /// </summary>
    /// <value>The duration of half of a cycle.</value>
    [Property("time")]
    [ValueConverter(typeof(TimeSpanToSecondsValueConverter))]
    public Option<TimeSpan> Time { get; set; }

    /// <summary>
    ///     Gets or sets the time before continuing rotation after one half of a cycle.
    /// </summary>
    /// <value>The wait time.</value>
    [Property("wait")]
    [ValueConverter(typeof(TimeSpanToSecondsValueConverter))]
    public Option<TimeSpan> Wait { get; set; }
}
