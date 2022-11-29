using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.MessageReceived" />.
/// </summary>
public sealed class MessageReceivedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MessageReceivedEventArgs" /> class.
    /// </summary>
    /// <param name="message">The received message.</param>
    public MessageReceivedEventArgs(VirtualParadiseMessage message)
    {
        Message = message;
    }

    /// <summary>
    ///     Gets the message.
    /// </summary>
    /// <value>The message.</value>
    public VirtualParadiseMessage Message { get; }
}
