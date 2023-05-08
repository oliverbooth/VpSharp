using System.Collections.Concurrent;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading.Channels;
using VpSharp.Entities;
using VpSharp.Exceptions;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using VpSharp.Internal.ValueConverters;
using static VpSharp.Internal.NativeMethods;

namespace VpSharp;

/// <summary>
///     Provides a managed API which offers full encapsulation of the native Virtual Paradise SDK.
/// </summary>
public sealed partial class VirtualParadiseClient : IDisposable
{
    private const string DefaultUniverseHost = "universe.virtualparadise.org";
    private const int DefaultUniversePort = 57000;
    private readonly ConcurrentDictionary<Cell, Channel<VirtualParadiseObject>> _cellChannels = new();

    private readonly VirtualParadiseConfiguration _configuration;

    private readonly ConcurrentDictionary<int, VirtualParadiseUser> _friends = new();
    private readonly Dictionary<int, TaskCompletionSource<ReasonCode>> _inviteCompletionSources = new();
    private readonly Dictionary<int, TaskCompletionSource<ReasonCode>> _joinCompletionSources = new();

    private TaskCompletionSource<ReasonCode>? _connectCompletionSource;
    private TaskCompletionSource<ReasonCode>? _enterCompletionSource;
    private TaskCompletionSource<ReasonCode>? _loginCompletionSource;

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseClient" /> class with the default configuration.
    /// </summary>
    public VirtualParadiseClient() : this(new VirtualParadiseConfiguration())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseClient" /> class with a specified configuration.
    /// </summary>
    /// <value>The configuration for this client.</value>
    /// <exception cref="ArgumentNullException"><paramref name="configuration" /> is <see langword="null" />.</exception>
    public VirtualParadiseClient(VirtualParadiseConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        Services = configuration.Services;
        _configuration = new VirtualParadiseConfiguration(configuration);
        Initialize();
    }

    /// <inheritdoc />
    ~VirtualParadiseClient()
    {
        Dispose();
    }

    /// <summary>
    ///     Gets a read-only view of the cached avatars.
    /// </summary>
    /// <value>The cached avatars.</value>
    public IReadOnlyList<VirtualParadiseAvatar> Avatars
    {
        get => _avatars.Values.ToArray();
    }

    /// <summary>
    ///     Gets the current avatar associated with this client.
    /// </summary>
    /// <value>
    ///     An instance of <see cref="VirtualParadiseAvatar" />, or <see langword="null" /> if this client is not in a world.
    /// </value>
    public VirtualParadiseAvatar? CurrentAvatar { get; internal set; }

    /// <summary>
    ///     Gets the current user to which this client is logged in.
    /// </summary>
    /// <value>
    ///     An instance of <see cref="VirtualParadiseUser" />, or <see langword="null" /> if this client is not logged in.
    /// </value>
    public VirtualParadiseUser? CurrentUser { get; internal set; }

    /// <summary>
    ///     Gets the world to which this client is currently connected.
    /// </summary>
    /// <value>
    ///     The world to which this client is currently connected, or <see langword="null" /> if this client is not currently
    ///     in a world.
    /// </value>
    public VirtualParadiseWorld? CurrentWorld
    {
        get => CurrentAvatar?.Location.World;
    }

    /// <summary>
    ///     Gets the service provider.
    /// </summary>
    /// <value>The service provider.</value>
    public IServiceProvider? Services { get; }

    /// <summary>
    ///     Gets a read-only view of the cached worlds.
    /// </summary>
    /// <value>The cached worlds.</value>
    public IReadOnlyList<VirtualParadiseWorld> Worlds
    {
        get => _worlds.Values.ToArray();
    }

    /// <summary>
    ///     Establishes a connection to the universe at the specified remote endpoint.
    /// </summary>
    /// <param name="host">The remote host.</param>
    /// <param name="port">The remote port.</param>
    /// <remarks>
    ///     If <paramref name="host" /> is <see langword="null" /> and/or <paramref name="port" /> is less than 1, the client
    ///     will use the default host and port values respectively.
    /// </remarks>
    public async Task ConnectAsync(string? host = null, int port = -1)
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            host = DefaultUniverseHost;
        }

        if (port < 1)
        {
            port = DefaultUniversePort;
        }

        ReasonCode reason;

        lock (Lock)
        {
            _connectCompletionSource = new TaskCompletionSource<ReasonCode>();

            reason = (ReasonCode)vp_connect_universe(NativeInstanceHandle, host, port);
            if (reason != ReasonCode.Success)
            {
                goto NoSuccess;
            }
        }

        reason = await _connectCompletionSource.Task.ConfigureAwait(false);

        NoSuccess:
        switch (reason)
        {
            case ReasonCode.Success:
                break;

            case ReasonCode.ConnectionError:
                throw new SocketException();

            default:
                throw new SocketException((int)reason);
        }
    }

    /// <summary>
    ///     Establishes a connection to the universe at the specified remote endpoint.
    /// </summary>
    /// <param name="remoteEP">The remote endpoint of the universe.</param>
    /// <exception cref="ArgumentNullException"><paramref name="remoteEP" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException"><paramref name="remoteEP" /> is not a supported endpoint.</exception>
    public Task ConnectAsync(EndPoint remoteEP)
    {
        if (remoteEP is null)
        {
            throw new ArgumentNullException(nameof(remoteEP));
        }

        string host;
        int port;

        switch (remoteEP)
        {
            case IPEndPoint ip:
                host = ip.Address.ToString();
                port = ip.Port;
                break;

            case DnsEndPoint dns:
                host = dns.Host;
                port = dns.Port;
                break;

            default:
                throw new ArgumentException(ExceptionMessages.UnsupportedEndpoint, nameof(remoteEP));
        }

        return ConnectAsync(host, port);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);

        DisposeCompletionSources();
        DisposeObservables();

        _worlds.Clear();
        _avatars.Clear();
        _users.Clear();
    }

    /// <summary>
    ///     Enters a specified world at a specified position.
    /// </summary>
    /// <param name="worldName">The name of the world to enter.</param>
    /// <param name="position">The position at which to enter the world.</param>
    /// <exception cref="ArgumentNullException"><paramref name="worldName" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     A world enter was attempted before the client was connected to a universe.
    /// </exception>
    /// <exception cref="Exception">Connection to the universe server was lost, or connecting to the world failed.</exception>
    /// <exception cref="WorldNotFoundException">The specified world was not found.</exception>
    /// <exception cref="TimeoutException">Connection to the world server timed out.</exception>
    public async Task<VirtualParadiseWorld> EnterAsync(string worldName, Vector3d position)
    {
        await EnterAsync(worldName).ConfigureAwait(false);
        await CurrentAvatar!.TeleportAsync(position, Rotation.None).ConfigureAwait(false);
        return CurrentWorld!;
    }

    /// <summary>
    ///     Enters a specified world at a specified position and rotation.
    /// </summary>
    /// <param name="worldName">The name of the world to enter.</param>
    /// <param name="position">The position at which to enter the world.</param>
    /// <param name="rotation">The rotation at which to enter the world.</param>
    /// <exception cref="ArgumentNullException"><paramref name="worldName" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     A world enter was attempted before the client was connected to a universe.
    /// </exception>
    /// <exception cref="Exception">Connection to the universe server was lost, or connecting to the world failed.</exception>
    /// <exception cref="WorldNotFoundException">The specified world was not found.</exception>
    /// <exception cref="TimeoutException">Connection to the world server timed out.</exception>
    public async Task<VirtualParadiseWorld> EnterAsync(string worldName, Vector3d position, Rotation rotation)
    {
        await EnterAsync(worldName).ConfigureAwait(false);
        await CurrentAvatar!.TeleportAsync(position, rotation).ConfigureAwait(false);
        return CurrentWorld!;
    }

    /// <summary>
    ///     Enters a specified world at a specified position.
    /// </summary>
    /// <param name="world">The world to enter.</param>
    /// <param name="position">The position at which to enter the world.</param>
    /// <exception cref="ArgumentNullException"><paramref name="world" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     A world enter was attempted before the client was connected to a universe.
    /// </exception>
    /// <exception cref="Exception">Connection to the universe server was lost, or connecting to the world failed.</exception>
    /// <exception cref="WorldNotFoundException">The specified world was not found.</exception>
    /// <exception cref="TimeoutException">Connection to the world server timed out.</exception>
    public async Task EnterAsync(VirtualParadiseWorld world, Vector3d position)
    {
        await EnterAsync(world).ConfigureAwait(false);
        await CurrentAvatar!.TeleportAsync(position, Rotation.None).ConfigureAwait(false);
    }

    /// <summary>
    ///     Enters a specified world at a specified position and rotation.
    /// </summary>
    /// <param name="world">The world to enter.</param>
    /// <param name="position">The position at which to enter the world.</param>
    /// <param name="rotation">The rotation at which to enter the world.</param>
    /// <exception cref="ArgumentNullException"><paramref name="world" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     A world enter was attempted before the client was connected to a universe.
    /// </exception>
    /// <exception cref="Exception">Connection to the universe server was lost, or connecting to the world failed.</exception>
    /// <exception cref="WorldNotFoundException">The specified world was not found.</exception>
    /// <exception cref="TimeoutException">Connection to the world server timed out.</exception>
    public async Task EnterAsync(VirtualParadiseWorld world, Vector3d position, Rotation rotation)
    {
        await EnterAsync(world).ConfigureAwait(false);
        await CurrentAvatar!.TeleportAsync(position, rotation).ConfigureAwait(false);
    }

    /// <summary>
    ///     Enters a specified world.
    /// </summary>
    /// <param name="world">The world to enter.</param>
    /// <exception cref="ArgumentNullException"><paramref name="world" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     A world enter was attempted before the client was connected to a universe.
    /// </exception>
    /// <exception cref="Exception">Connection to the universe server was lost, or connecting to the world failed.</exception>
    /// <exception cref="WorldNotFoundException">The specified world was not found.</exception>
    /// <exception cref="TimeoutException">Connection to the world server timed out.</exception>
    public async Task EnterAsync(VirtualParadiseWorld world)
    {
        ArgumentNullException.ThrowIfNull(world);
        await EnterAsync(world.Name).ConfigureAwait(false);
    }

    /// <summary>
    ///     Enters a specified world.
    /// </summary>
    /// <param name="worldName">The world to enter.</param>
    /// <returns>A <see cref="VirtualParadiseWorld" /> representing the world.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="worldName" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     A world enter was attempted before the client was connected to a universe.
    /// </exception>
    /// <exception cref="ArgumentException">The specified world name is too long.</exception>
    /// <exception cref="Exception">Connection to the universe server was lost, or connecting to the world failed.</exception>
    /// <exception cref="WorldNotFoundException">The specified world was not found.</exception>
    /// <exception cref="TimeoutException">Connection to the world server timed out.</exception>
    public async Task<VirtualParadiseWorld> EnterAsync(string worldName)
    {
        ArgumentNullException.ThrowIfNull(worldName);

        if (CurrentWorld is not null)
        {
            lock (Lock)
            {
                _ = vp_leave(NativeInstanceHandle);
            }
        }

        ReasonCode reason;

        _worldSettingsCompletionSource = new TaskCompletionSource();
        _enterCompletionSource = new TaskCompletionSource<ReasonCode>();

        lock (Lock)
        {
            reason = (ReasonCode)vp_enter(NativeInstanceHandle, worldName);
            if (reason != ReasonCode.Success)
            {
                goto NoSuccess;
            }
        }

        reason = await _enterCompletionSource.Task.ConfigureAwait(false);

        NoSuccess:
        switch (reason)
        {
            case ReasonCode.Success:
                break;

            case ReasonCode.NotInUniverse:
                throw new InvalidOperationException(ExceptionMessages.ConnectionToUniverseServerRequired);

            case ReasonCode.StringTooLong:
                ThrowHelper.ThrowStringTooLongException(nameof(worldName));
                break;

            case ReasonCode.ConnectionError:
            case ReasonCode.WorldLoginError:
                throw new VirtualParadiseException(reason, ExceptionMessages.WorldLoginError);

            case ReasonCode.WorldNotFound:
                throw new WorldNotFoundException(worldName);

            case ReasonCode.Timeout:
                throw new TimeoutException(ExceptionMessages.ConnectionTimedOut);

            default:
                throw new VirtualParadiseException(reason);
        }

        int size;
        lock (Lock)
        {
            size = vp_int(NativeInstanceHandle, IntegerAttribute.WorldSize);
        }

        await _worldSettingsCompletionSource.Task.ConfigureAwait(false);

        VirtualParadiseWorld? world = await GetWorldAsync(worldName).ConfigureAwait(false);
        if (world is null)
        {
            // we entered the world but it wasn't listed. unlisted world. we'll try our best to create details for it
            world = new VirtualParadiseWorld(this, worldName);
        }

        if (CurrentAvatar is not null)
        {
            CurrentAvatar.Location = new Location(world);
        }

        lock (Lock)
        {
            _ = vp_state_change(NativeInstanceHandle);
        }

        world.Size = new Size(size, size);

        if (CurrentWorld is not null)
        {
            CurrentWorld.Settings = WorldSettingsConverter.FromDictionary(_worldSettings);
            _worldSettings.Clear();
        }

        CurrentAvatar = new VirtualParadiseAvatar(this, -1)
        {
            Application = _configuration.Application!,
            Name = $"[{_configuration.BotName}]",
            Location = new Location(world, Vector3d.Zero, Rotation.None),
            User = CurrentUser!
        };

        if (_configuration.AutoQuery)
        {
            _ = Task.Run(async () =>
            {
                await foreach (VirtualParadiseObject virtualParadiseObject in EnumerateObjectsAsync(default, size))
                {
                    AddOrUpdateObject(virtualParadiseObject);
                }
            });
        }

        return CurrentWorld!;
    }

    /// <summary>
    ///     Leaves the current world.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     An attempt was made to leave a world when the client was not present in one.
    /// </exception>
    public Task LeaveAsync()
    {
        lock (Lock)
        {
            var reason = (ReasonCode)vp_leave(NativeInstanceHandle);
            if (reason == ReasonCode.NotInWorld)
            {
                return Task.FromException(ThrowHelper.NotInWorldException());
            }
        }

        _avatars.Clear();
        _objects.Clear();
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Logs in to the current universe.
    /// </summary>
    /// <exception cref="ArgumentException">
    ///     <para>
    ///         <see cref="VirtualParadiseConfiguration.Username" /> is <see langword="null" />, empty, or consists only of
    ///         whitespace.
    ///     </para>
    ///     -or-
    ///     <para>
    ///         <see cref="VirtualParadiseConfiguration.Password" /> is <see langword="null" />, empty, or consists only of
    ///         whitespace.
    ///     </para>
    ///     -or-
    ///     <para>
    ///         <see cref="VirtualParadiseConfiguration.BotName" /> is <see langword="null" />, empty, or consists only of
    ///         whitespace.
    ///     </para>
    ///     -or-
    ///     <para>
    ///         A value in the configuration is too long. (Most likely <see cref="VirtualParadiseConfiguration.BotName" />).
    ///     </para>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     A login was attempted before a connection with the universe server was established.
    /// </exception>
    /// <exception cref="TimeoutException">The login request timed out.</exception>
    /// <exception cref="InvalidCredentialException">The specified username and password constitute an invalid login.</exception>
    public async Task LoginAsync(string? username = null, string? password = null, string? botName = null)
    {
        username ??= _configuration.Username;
        password ??= _configuration.Password;
        botName ??= _configuration.BotName;

        _configuration.Username = username;
        _configuration.Password = password;
        _configuration.BotName = botName;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(botName))
        {
            throw new ArgumentException(ExceptionMessages.CannotLogin_IncompleteConfiguration);
        }

        _loginCompletionSource = new TaskCompletionSource<ReasonCode>();

        ReasonCode reason;
        lock (Lock)
        {
            if (_configuration.Application is var (name, version))
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    vp_string_set(NativeInstanceHandle, StringAttribute.ApplicationName, name);
                    vp_string_set(NativeInstanceHandle, StringAttribute.ApplicationVersion, version ?? string.Empty);
                }
            }

            reason = (ReasonCode)vp_login(NativeInstanceHandle, username, password, botName);
            if (reason != ReasonCode.Success)
            {
                goto NoSuccess;
            }
        }

        reason = await _loginCompletionSource.Task.ConfigureAwait(false);
        NoSuccess:
        switch (reason)
        {
            case ReasonCode.Timeout:
                throw new TimeoutException(ExceptionMessages.CannotLogin_Timeout);

            case ReasonCode.InvalidLogin:
                throw new InvalidCredentialException(ExceptionMessages.CannotLogin_InvalidLogin);

            case ReasonCode.StringTooLong:
                throw new ArgumentException(ExceptionMessages.CannotLogin_StringTooLong);

            case ReasonCode.NotInUniverse:
                throw new InvalidOperationException(ExceptionMessages.ConnectionToUniverseServerRequired);
        }

        int userId;

        lock (Lock)
        {
            userId = vp_int(NativeInstanceHandle, IntegerAttribute.MyUserId);
        }

        CurrentUser = await GetUserAsync(userId).ConfigureAwait(false);
        _ = vp_friends_get(NativeInstanceHandle);
    }

    /// <summary>
    ///     Sends a chat message to everyone in the current world.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns>The message which was sent.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     An attempt was made to send a message while not connected to a world.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <para><paramref name="message" /> is empty, or consists of only whitespace.</para>
    ///     -or-
    ///     <para><paramref name="message" /> is too long to send.</para>
    /// </exception>
    public Task<VirtualParadiseMessage> SendMessageAsync(string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException(ExceptionMessages.ValueCannotBeEmpty, nameof(message));
        }

        lock (Lock)
        {
            var reason = (ReasonCode)vp_say(NativeInstanceHandle, message);
            if (reason != ReasonCode.Success)
            {
                switch (reason)
                {
                    case ReasonCode.NotInWorld:
                        throw new InvalidOperationException(ExceptionMessages.ConnectionToWorldServerRequired);

                    case ReasonCode.StringTooLong:
                        throw new ArgumentException(ExceptionMessages.StringTooLong);
                }
            }
        }

        VirtualParadiseAvatar? avatar = CurrentAvatar;
        return Task.FromResult(new VirtualParadiseMessage(
            MessageType.ChatMessage,
            avatar!.Name,
            message,
            avatar,
            FontStyle.Regular,
            Color.Black
        ));
    }

    /// <summary>
    ///     Sends a console message to everyone in the current world.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="fontStyle">The font style of the message.</param>
    /// <param name="color">The text color of the message.</param>
    /// <returns>The message which was sent.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     An attempt was made to send a message while not connected to a world.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <para><paramref name="message" /> is empty, or consists of only whitespace.</para>
    ///     -or-
    ///     <para><paramref name="message" /> is too long to send.</para>
    /// </exception>
    public Task<VirtualParadiseMessage> SendMessageAsync(string message, FontStyle fontStyle, Color color)
    {
        return SendMessageAsync(null, message, fontStyle, color);
    }

    /// <summary>
    ///     Sends a console message to everyone in the current world.
    /// </summary>
    /// <param name="name">The apparent author of the message.</param>
    /// <param name="message">The message to send.</param>
    /// <param name="fontStyle">The font style of the message.</param>
    /// <param name="color">The text color of the message.</param>
    /// <returns>The message which was sent.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     An attempt was made to send a message while not connected to a world.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <para><paramref name="message" /> is empty, or consists of only whitespace.</para>
    ///     -or-
    ///     <para><paramref name="message" /> is too long to send.</para>
    /// </exception>
    public Task<VirtualParadiseMessage> SendMessageAsync(string? name, string message, FontStyle fontStyle, Color color)
    {
        ArgumentNullException.ThrowIfNull(message);
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException(ExceptionMessages.ValueCannotBeEmpty, nameof(message));
        }

        lock (Lock)
        {
            var reason = (ReasonCode)vp_console_message(
                NativeInstanceHandle,
                0,
                name ?? string.Empty,
                message,
                (int)fontStyle,
                color.R,
                color.G,
                color.B
            );

            if (reason != ReasonCode.Success)
            {
                switch (reason)
                {
                    case ReasonCode.NotInWorld:
                        throw new InvalidOperationException(ExceptionMessages.ConnectionToWorldServerRequired);

                    case ReasonCode.StringTooLong:
                        throw new ArgumentException(ExceptionMessages.StringTooLong);
                }
            }
        }

        VirtualParadiseAvatar avatar = CurrentAvatar!;
        return Task.FromResult(new VirtualParadiseMessage(
            MessageType.ConsoleMessage,
            name,
            message,
            avatar,
            fontStyle,
            color
        ));
    }

    internal TaskCompletionSource<ReasonCode> AddJoinCompletionSource(int reference)
    {
        var taskCompletionSource = new TaskCompletionSource<ReasonCode>();
        _joinCompletionSources.TryAdd(reference, taskCompletionSource);
        return taskCompletionSource;
    }

    internal TaskCompletionSource<ReasonCode> AddInviteCompletionSource(int reference)
    {
        var taskCompletionSource = new TaskCompletionSource<ReasonCode>();
        _inviteCompletionSources.TryAdd(reference, taskCompletionSource);
        return taskCompletionSource;
    }

    private void DisposeCompletionSources()
    {
        _connectCompletionSource?.TrySetCanceled();
        _connectCompletionSource = null;

        _enterCompletionSource?.TrySetCanceled();
        _enterCompletionSource = null;

        _loginCompletionSource?.TrySetCanceled();
        _loginCompletionSource = null;

        _worldSettingsCompletionSource.TrySetCanceled();
        _worldSettingsCompletionSource = null!;

        foreach (KeyValuePair<int, TaskCompletionSource<ReasonCode>> pair in _inviteCompletionSources)
        {
            pair.Value.TrySetCanceled();
        }

        foreach (KeyValuePair<int, TaskCompletionSource<ReasonCode>> pair in _joinCompletionSources)
        {
            pair.Value.TrySetCanceled();
        }

        foreach (KeyValuePair<int, TaskCompletionSource<(ReasonCode, VirtualParadiseObject?)>> pair in _objectCompletionSources)
        {
            pair.Value.TrySetCanceled();
        }

        foreach (KeyValuePair<int, TaskCompletionSource<ReasonCode>> pair in _objectUpdates)
        {
            pair.Value.TrySetCanceled();
        }

        foreach (KeyValuePair<int, TaskCompletionSource<VirtualParadiseUser>> pair in _usersCompletionSources)
        {
            pair.Value.TrySetCanceled();
        }

        _joinCompletionSources.Clear();
        _inviteCompletionSources.Clear();
        _objectCompletionSources.Clear();
        _objectUpdates.Clear();
        _usersCompletionSources.Clear();
    }

    private void DisposeObservables()
    {
        _avatarClicked.Dispose();
        _avatarJoined.Dispose();
        _avatarLeft.Dispose();
        _avatarMoved.Dispose();
        _avatarTypeChanged.Dispose();

        _objectBump.Dispose();
        _objectClicked.Dispose();
        _objectCreated.Dispose();
        _objectDeleted.Dispose();
        _objectUpdates.Clear();

        _messageReceived.Dispose();
        _inviteRequestReceived.Dispose();
        _joinRequestReceived.Dispose();
        _teleported.Dispose();
        _uriReceived.Dispose();
        _universeServerDisconnected.Dispose();
        _worldServerDisconnected.Dispose();
    }

    private void ReleaseUnmanagedResources()
    {
        _ = vp_destroy(NativeInstanceHandle);
        _instanceHandle.Free();
    }
}
