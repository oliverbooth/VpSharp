using VpSharp.Building.Annotations;
using VpSharp.Building.Commands.Converters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>name</c> command.
/// </summary>
[Command("name")]
public sealed class NameCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the name value.
    /// </summary>
    /// <value>The name value.</value>
    [Parameter(0)]
    public string Name { get; set; } = string.Empty;
}
