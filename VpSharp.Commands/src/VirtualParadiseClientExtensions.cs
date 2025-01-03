namespace VpSharp.Commands;

/// <summary>
///     Extension methods for <see cref="VirtualParadiseClient" />.
/// </summary>
public static class VirtualParadiseClientExtensions
{
    /// <summary>
    ///     Registers <see cref="CommandsExtension" /> to be used with the current client.
    /// </summary>
    /// <param name="client">The <see cref="VirtualParadiseClient" />.</param>
    /// <param name="configuration">The configuration required for the extensions.</param>
    /// <returns>The commands extension instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="client" /> is <see langword="null" />.</exception>
    public static CommandsExtension UseCommands(this VirtualParadiseClient client, CommandsExtensionConfiguration configuration)
    {
        if (client is null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        return client.AddExtension<CommandsExtension>(configuration);
    }
}
