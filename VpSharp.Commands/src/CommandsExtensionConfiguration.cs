namespace VpSharp.Commands;

/// <summary>
///     Defines configuration for <see cref="CommandsExtensionConfiguration" />.
/// </summary>
public sealed class CommandsExtensionConfiguration
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandsExtensionConfiguration" /> class.
    /// </summary>
    /// <param name="configuration">The configuration to copy.</param>
    public CommandsExtensionConfiguration(CommandsExtensionConfiguration? configuration = null)
    {
        if (configuration is null)
        {
            return;
        }

        Prefixes = configuration.Prefixes;
        Services = configuration.Services;
    }

    /// <summary>
    ///     Gets or sets the prefixes to be use for commands.
    /// </summary>
    /// <value>The command prefixes, as an array of <see cref="string" /> values.</value>
    public IReadOnlyList<string> Prefixes { get; set; } = Array.Empty<string>();

    /// <summary>
    ///     Gets or sets the service provider.
    /// </summary>
    /// <value>The service provider.</value>
    public IServiceProvider? Services { get; set; }
}
