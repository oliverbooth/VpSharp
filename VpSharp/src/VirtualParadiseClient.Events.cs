using VpSharp.EventData;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    /// <summary>
    ///     Occurs when an avatar has clicked another avatar.
    /// </summary>
    public event EventHandler<AvatarClickedEventArgs>? AvatarClicked;

    /// <summary>
    ///     Occurs when an avatar has entered the vicinity of the client.
    /// </summary>
    public event EventHandler<AvatarJoinedEventArgs>? AvatarJoined;

    /// <summary>
    ///     Occurs when an avatar has left the vicinity of the client.
    /// </summary>
    public event EventHandler<AvatarLeftEventArgs>? AvatarLeft;

    /// <summary>
    ///     Occurs when an avatar has changed their position or rotation.
    /// </summary>
    public event EventHandler<AvatarMovedEventArgs>? AvatarMoved;

    /// <summary>
    ///     Occurs when an avatar has changed their type.
    /// </summary>
    public event EventHandler<AvatarTypeChangedEventArgs>? AvatarTypeChanged;

    /// <summary>
    ///     Occurs when an invite request has been received.
    /// </summary>
    public event EventHandler<InviteRequestReceivedEventArgs>? InviteRequestReceived;

    /// <summary>
    ///     Occurs when a join request has been received.
    /// </summary>
    public event EventHandler<JoinRequestReceivedEventArgs>? JoinRequestReceived;

    /// <summary>
    ///     Occurs when a chat message or console message has been received.
    /// </summary>
    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;

    /// <summary>
    ///     Occurs when a bump phase has changed for an object.
    /// </summary>
    public event EventHandler<ObjectBumpEventArgs>? ObjectBump;

    /// <summary>
    ///     Occurs when an object has been changed.
    /// </summary>
    public event EventHandler<ObjectChangedEventArgs>? ObjectChanged;

    /// <summary>
    ///     Occurs when an avatar has clicked an object.
    /// </summary>
    public event EventHandler<ObjectClickedEventArgs>? ObjectClicked;

    /// <summary>
    ///     Occurs when an object has been created.
    /// </summary>
    public event EventHandler<ObjectCreatedEventArgs>? ObjectCreated;

    /// <summary>
    ///     Occurs when an object has been deleted.
    /// </summary>
    public event EventHandler<ObjectDeletedEventArgs>? ObjectDeleted;

    /// <summary>
    ///     Occurs when an avatar has requested this client to teleport.
    /// </summary>
    public event EventHandler<TeleportedEventArgs>? Teleported;

    /// <summary>
    ///     Occurs when the client has been disconnected from the universe server.
    /// </summary>
    public event EventHandler<DisconnectedEventArgs>? UniverseServerDisconnected;

    /// <summary>
    ///     Occurs when an avatar has sent a URI to this client.
    /// </summary>
    public event EventHandler<UriReceivedEventArgs>? UriReceived;

    /// <summary>
    ///     Occurs when the client has been disconnected from the world server.
    /// </summary>
    public event EventHandler<DisconnectedEventArgs>? WorldServerDisconnected;

    private void RaiseEvent<T>(EventHandler<T>? eventHandler, T args)
        where T : EventArgs
    {
        eventHandler?.Invoke(this, args);
    }
}
