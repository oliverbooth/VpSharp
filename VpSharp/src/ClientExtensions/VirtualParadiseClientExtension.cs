using VpSharp.Entities;

namespace VpSharp.ClientExtensions;

/// <summary>
///     Represents the base class for extensions to the <see cref="VirtualParadiseClient" />.
/// </summary>
public abstract class VirtualParadiseClientExtension
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseClientExtension" /> class.
    /// </summary>
    /// <param name="client">The owning client.</param>
    /// <exception cref="ArgumentNullException"><paramref name="client" /> is <see langword="null" />.</exception>
    protected VirtualParadiseClientExtension(VirtualParadiseClient client)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <summary>
    ///     Gets the client to which this extension is registered.
    /// </summary>
    /// <value>The owning client.</value>
    public VirtualParadiseClient Client { get; }

    /// <summary>
    ///     Called when a chat message is received.
    /// </summary>
    /// <param name="message">The message which was received.</param>
    protected internal virtual Task OnMessageReceived(IMessage message)
    {
        return Task.CompletedTask;
    }
}
