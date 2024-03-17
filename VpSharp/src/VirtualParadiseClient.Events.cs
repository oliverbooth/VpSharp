using System.Reactive.Subjects;
using VpSharp.Entities;
using VpSharp.EventData;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly Subject<AvatarClickedEventArgs> _avatarClicked = new();
    private readonly Subject<IAvatar> _avatarJoined = new();
    private readonly Subject<IAvatar> _avatarLeft = new();
    private readonly Subject<AvatarMovedEventArgs> _avatarMoved = new();
    private readonly Subject<AvatarTypeChangedEventArgs> _avatarTypeChanged = new();
    private readonly Subject<InviteRequest> _inviteRequestReceived = new();
    private readonly Subject<JoinRequest> _joinRequestReceived = new();
    private readonly Subject<IMessage> _messageReceived = new();
    private readonly Subject<ObjectBumpEventArgs> _objectBump = new();
    private readonly Subject<ObjectChangedEventArgs> _objectChanged = new();
    private readonly Subject<ObjectClickedEventArgs> _objectClicked = new();
    private readonly Subject<ObjectCreatedEventArgs> _objectCreated = new();
    private readonly Subject<ObjectDeletedEventArgs> _objectDeleted = new();
    private readonly Subject<TeleportedEventArgs> _teleported = new();
    private readonly Subject<DisconnectReason> _universeServerDisconnected = new();
    private readonly Subject<UriReceivedEventArgs> _uriReceived = new();
    private readonly Subject<DisconnectReason> _worldServerDisconnected = new();

    /// <summary>
    ///     Occurs when an avatar has clicked another avatar.
    /// </summary>
    public IObservable<AvatarClickedEventArgs> AvatarClicked
    {
        get => _avatarClicked;
    }

    /// <summary>
    ///     Occurs when an avatar has entered the vicinity of the client.
    /// </summary>
    public IObservable<IAvatar> AvatarJoined
    {
        get => _avatarJoined;
    }

    /// <summary>
    ///     Occurs when an avatar has left the vicinity of the client.
    /// </summary>
    public IObservable<IAvatar> AvatarLeft
    {
        get => _avatarLeft;
    }

    /// <summary>
    ///     Occurs when an avatar has changed their position or rotation.
    /// </summary>
    public IObservable<AvatarMovedEventArgs> AvatarMoved
    {
        get => _avatarMoved;
    }

    /// <summary>
    ///     Occurs when an avatar has changed their type.
    /// </summary>
    public IObservable<AvatarTypeChangedEventArgs> AvatarTypeChanged
    {
        get => _avatarTypeChanged;
    }

    /// <summary>
    ///     Occurs when an invite request has been received.
    /// </summary>
    public IObservable<InviteRequest> InviteRequestReceived
    {
        get => _inviteRequestReceived;
    }

    /// <summary>
    ///     Occurs when a join request has been received.
    /// </summary>
    public IObservable<JoinRequest> JoinRequestReceived
    {
        get => _joinRequestReceived;
    }

    /// <summary>
    ///     Occurs when a chat message or console message has been received.
    /// </summary>
    public IObservable<IMessage> MessageReceived
    {
        get => _messageReceived;
    }

    /// <summary>
    ///     Occurs when a bump phase has changed for an object.
    /// </summary>
    public IObservable<ObjectBumpEventArgs> ObjectBump
    {
        get => _objectBump;
    }

    /// <summary>
    ///     Occurs when an object has been changed.
    /// </summary>
    public IObservable<ObjectChangedEventArgs> ObjectChanged
    {
        get => _objectChanged;
    }

    /// <summary>
    ///     Occurs when an avatar has clicked an object.
    /// </summary>
    public IObservable<ObjectClickedEventArgs> ObjectClicked
    {
        get => _objectClicked;
    }

    /// <summary>
    ///     Occurs when an object has been created.
    /// </summary>
    public IObservable<ObjectCreatedEventArgs> ObjectCreated
    {
        get => _objectCreated;
    }

    /// <summary>
    ///     Occurs when an object has been deleted.
    /// </summary>
    public IObservable<ObjectDeletedEventArgs> ObjectDeleted
    {
        get => _objectDeleted;
    }

    /// <summary>
    ///     Occurs when an avatar has requested this client to teleport.
    /// </summary>
    public IObservable<TeleportedEventArgs> Teleported
    {
        get => _teleported;
    }

    /// <summary>
    ///     Occurs when the client has been disconnected from the universe server.
    /// </summary>
    public IObservable<DisconnectReason> UniverseServerDisconnected
    {
        get => _universeServerDisconnected;
    }

    /// <summary>
    ///     Occurs when an avatar has sent a URI to this client.
    /// </summary>
    public IObservable<UriReceivedEventArgs> UriReceived
    {
        get => _uriReceived;
    }


    /// <summary>
    ///     Occurs when the client has been disconnected from the world server.
    /// </summary>
    public IObservable<DisconnectReason> WorldServerDisconnected
    {
        get => _worldServerDisconnected;
    }
}
