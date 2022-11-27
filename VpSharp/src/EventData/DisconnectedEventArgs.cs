namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.WorldServerDisconnected" /> and
///     <see cref="VirtualParadiseClient.UniverseServerDisconnected" />.
/// </summary>
public sealed class DisconnectedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DisconnectedEventArgs" /> class.
    /// </summary>
    /// <param name="reason">The reason for the disconnect.</param>
    public DisconnectedEventArgs(DisconnectReason reason)
    {
        Reason = reason;
    }

    /// <summary>
    ///     Gets the reason for the disconnect.
    /// </summary>
    /// <value>The reason for the disconnect.</value>
    public DisconnectReason Reason { get; }
}