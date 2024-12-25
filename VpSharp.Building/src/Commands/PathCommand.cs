using Optional;
using VpSharp.Building.Annotations;
using VpSharp.Building.ValueConverters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>path</c> command.
/// </summary>
[Command("path")]
public sealed class PathCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the name of the path to follow.
    /// </summary>
    /// <value>The name of the path.</value>
    [Parameter(0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the duration after which the path should be reset.
    /// </summary>
    /// <value>The duration after which the path should be reset.</value>
    [Property("resetafter")]
    [ValueConverter(typeof(TimeSpanToSecondsValueConverter))]
    public Option<TimeSpan> ResetAfter { get; set; }
}
