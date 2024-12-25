using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>url</c> command.
/// </summary>
[Command("url")]
public sealed class UrlCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    [Parameter(0)]
    public string Url { get; set; } = string.Empty;
}
