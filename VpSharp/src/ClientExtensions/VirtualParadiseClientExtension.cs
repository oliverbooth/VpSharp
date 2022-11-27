﻿using VpSharp.EventData;

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
    protected VirtualParadiseClientExtension(VirtualParadiseClient client)
    {
        Client = client;
    }

    /// <summary>
    ///     Gets the client to which this extension is registered.
    /// </summary>
    /// <value>The owning client.</value>
    public VirtualParadiseClient Client { get; }

    /// <summary>
    ///     Called when a chat message is received.
    /// </summary>
    /// <param name="args">An object containing event data.</param>
    protected internal virtual Task OnMessageReceived(MessageReceivedEventArgs args)
    {
        return Task.CompletedTask;
    }
}
