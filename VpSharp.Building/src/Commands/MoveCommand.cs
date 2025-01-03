using VpSharp.Building.Annotations;
using VpSharp.Building.Serialization.CommandConverters;
using VpSharp.Building.Serialization.ValueConverters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>move</c> command.
/// </summary>
[Command("move", ConverterType = typeof(MoveCommandConverter))]
public sealed class MoveCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the move value.
    /// </summary>
    /// <value>The move value.</value>
    [Parameter(0)]
    public Vector3d Movement { get; set; } = Vector3d.Zero;

    /// <summary>
    ///     Gets or sets a value indicating whether this movement is along the object axis (as opposed to world axis).
    /// </summary>
    /// <value><see langword="true" /> if this movement is along the object axis; otherwise, <see langword="false" />.</value>
    [Flag("ltm")]
    public bool IsLocalAxis { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this movement loops.
    /// </summary>
    /// <value><see langword="true" /> if this movement loops; otherwise, <see langword="false" />.</value>
    [Flag("loop")]
    public bool IsLooping { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this movement is smooth.
    /// </summary>
    /// <value><see langword="true" /> if this movement is smooth; otherwise, <see langword="false" />.</value>
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
    ///     Gets or sets a value indicating whether this movement resets after completing half a cycle.
    /// </summary>
    /// <value><see langword="true" /> if this movement resets; otherwise, <see langword="false" />.</value>
    [Flag("reset")]
    public bool ShouldReset { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this movement syncs.
    /// </summary>
    /// <value><see langword="true" /> if this movement syncs; otherwise, <see langword="false" />.</value>
    [Flag("sync")]
    public bool ShouldSync { get; set; }

    /// <summary>
    ///     Gets or sets the duration of half of a cycle.
    /// </summary>
    /// <value>The duration of half of a cycle.</value>
    [Property("time")]
    [ValueConverter(typeof(TimeSpanToSecondsValueConverter))]
    public TimeSpan Time { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    ///     Gets or sets the time before continuing movement after one half of a cycle.
    /// </summary>
    /// <value>The wait time.</value>
    [Property("wait")]
    [ValueConverter(typeof(TimeSpanToSecondsValueConverter))]
    public TimeSpan Wait { get; set; }
}
