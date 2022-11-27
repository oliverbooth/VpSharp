namespace VpSharp.Commands;

/// <summary>
///     Defines configuration for <see cref="CommandsExtensionConfiguration" />.
/// </summary>
public sealed class CommandsExtensionConfiguration
{
    /// <summary>
    ///     Gets or sets the prefixes to be use for commands.
    /// </summary>
    /// <value>The command prefixes, as an array of <see cref="string" /> values.</value>
    public string[] Prefixes { get; set; } = Array.Empty<string>();
}
