using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>say</c> command.
/// </summary>
[Command("say")]
public sealed class SayCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the sign text.
    /// </summary>
    /// <value>The sign text.</value>
    [Parameter(0)]
    public string Message { get; set; } = string.Empty;
}
