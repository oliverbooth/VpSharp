using VpSharp.Building.Commands.Converters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>name</c> command.
/// </summary>
[Command("name", ConverterType = typeof(NameCommandConverter))]
public sealed class NameCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the name value.
    /// </summary>
    /// <value>The name value.</value>
    public string Name { get; set; } = string.Empty;
}
