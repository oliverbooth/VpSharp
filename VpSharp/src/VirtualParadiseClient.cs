using System.Collections.Concurrent;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Authentication;
using System.Threading.Channels;
using VpSharp.Entities;
using VpSharp.Exceptions;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using VpSharp.Internal.ValueConverters;
using static VpSharp.Internal.Native;

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
    public VirtualParadiseClient(VirtualParadiseConfiguration configuration)
    {
        _configuration = new VirtualParadiseConfiguration(configuration);
        Initialize();
    }

    /// <inheritdoc />
    ~VirtualParadiseClient()
    {
        Dispose();
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
        await CurrentAvatar!.TeleportAsync(position, Quaternion.Identity).ConfigureAwait(false);
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
    public async Task<VirtualParadiseWorld> EnterAsync(string worldName, Vector3d position, Quaternion rotation)
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
        await CurrentAvatar!.TeleportAsync(position, Quaternion.Identity).ConfigureAwait(false);
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
    public async Task EnterAsync(VirtualParadiseWorld world, Vector3d position, Quaternion rotation)
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
                vp_leave(NativeInstanceHandle);
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
                throw new InvalidOperationException(
                    "The client must be connected to the universe server in order to enter a world.");

            case ReasonCode.StringTooLong:
                ThrowHelper.ThrowStringTooLongException(nameof(worldName));
                break;

            case ReasonCode.ConnectionError:
            case ReasonCode.WorldLoginError:
                throw new Exception("Connection to the universe server was lost, or connecting to the world failed.");

            case ReasonCode.WorldNotFound:
                throw new WorldNotFoundException(worldName);

            case ReasonCode.Timeout:
                throw new TimeoutException("Connection to the world server timed out.");

            default:
                throw new Exception($"Unknown error: {reason:D} ({reason:G})");
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
            vp_state_change(NativeInstanceHandle);
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
            Location = new Location(world, Vector3d.Zero, Quaternion.Identity),
            User = CurrentUser!
        };

        if (_configuration.AutoQuery)
        {
            _ = Task.Run(async () =>
            {
                await foreach (VirtualParadiseObject virtualParadiseObject in EnumerateObjectsAsync(default, radius: size))
                {
                    AddOrUpdateObject(virtualParadiseObject);
                }
            });
        }

        return CurrentWorld!;
    }

    /// <summary>
    ///     Gets an enumerable of the worlds returned by the universe server. 
    /// </summary>
    /// <returns>An <see cref="IAsyncEnumerable{T}" /> containing <see cref="VirtualParadiseWorld" /> values.</returns>
    /// <remarks>
    ///     This method will yield results back as they are received from the world server. To access a consumed collection,
    ///     use <see cref="GetWorldsAsync" />.
    /// </remarks>
    /// <seealso cref="GetWorldsAsync" />
    public IAsyncEnumerable<VirtualParadiseWorld> EnumerateWorldsAsync()
    {
        _worldListChannel = Channel.CreateUnbounded<VirtualParadiseWorld>();
        lock (Lock)
        {
            vp_world_list(NativeInstanceHandle, 0);
        }

        return _worldListChannel.Reader.ReadAllAsync();
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
            throw new ArgumentException("Cannot login due to incomplete configuration.");
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
                throw new TimeoutException("The login request timed out.");

            case ReasonCode.InvalidLogin:
                throw new InvalidCredentialException("The specified username and password constitute an invalid login.");

            case ReasonCode.StringTooLong:
                throw new ArgumentException($"A value in the configuration is too long. ({nameof(_configuration.BotName)}?)");

            case ReasonCode.NotInUniverse:
                throw new InvalidOperationException("A connection to the universe server is required to attempt login.");
        }

        int userId;

        lock (Lock)
        {
            userId = vp_int(NativeInstanceHandle, IntegerAttribute.MyUserId);
        }

        CurrentUser = await GetUserAsync(userId).ConfigureAwait(false);
        vp_friends_get(NativeInstanceHandle);
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
            throw new ArgumentException("Message cannot be empty.");
        }

        lock (Lock)
        {
            var reason = (ReasonCode)vp_say(NativeInstanceHandle, message);
            if (reason != ReasonCode.Success)
            {
                switch (reason)
                {
                    case ReasonCode.NotInWorld:
                        throw new InvalidOperationException("A connection to the world server is required to send messages.");

                    case ReasonCode.StringTooLong:
                        throw new ArgumentException("The message is too long to send.");
                }
            }
        }

        var avatar = CurrentAvatar;
        return Task.FromResult(new VirtualParadiseMessage(
            MessageType.ChatMessage,
            avatar!.Name,
            message,
            avatar,
            FontStyle.Regular,
            Color.Black
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

    private void ReleaseUnmanagedResources()
    {
        vp_destroy(NativeInstanceHandle);
        _instanceHandle.Free();
    }
}
