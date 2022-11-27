namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.InviteRequestReceived" />.
/// </summary>
public sealed class InviteRequestReceivedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InviteRequestReceivedEventArgs" /> class.
    /// </summary>
    /// <param name="inviteRequest">The invite request.</param>
    public InviteRequestReceivedEventArgs(InviteRequest inviteRequest)
    {
        InviteRequest = inviteRequest;
    }

    /// <summary>
    ///     Gets the invite request.
    /// </summary>
    /// <value>The invite request.</value>
    public InviteRequest InviteRequest { get; }
}