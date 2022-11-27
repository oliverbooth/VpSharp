namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.JoinRequestReceived" />.
/// </summary>
public sealed class JoinRequestReceivedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="JoinRequestReceivedEventArgs" /> class.
    /// </summary>
    /// <param name="joinRequest">The join request.</param>
    public JoinRequestReceivedEventArgs(JoinRequest joinRequest)
    {
        JoinRequest = joinRequest;
    }

    /// <summary>
    ///     Gets the join request.
    /// </summary>
    /// <value>The join request.</value>
    public JoinRequest JoinRequest { get; }
}