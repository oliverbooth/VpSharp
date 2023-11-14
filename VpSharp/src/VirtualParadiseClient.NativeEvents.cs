using System.Collections.Concurrent;
using System.Drawing;
using System.Threading.Channels;
using VpSharp.ClientExtensions;
using VpSharp.Entities;
using VpSharp.EventData;
using VpSharp.Exceptions;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using static VpSharp.Internal.NativeMethods;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly ConcurrentDictionary<NativeEvent, NativeEventHandler> _nativeEventHandlers = new();

    private void SetNativeEvents()
    {
        SetNativeEvent(NativeEvent.Chat, OnChatNativeEvent);
        SetNativeEvent(NativeEvent.AvatarAdd, OnAvatarAddNativeEvent);
        SetNativeEvent(NativeEvent.AvatarChange, OnAvatarChangeNativeEvent);
        SetNativeEvent(NativeEvent.AvatarDelete, OnAvatarDeleteNativeEvent);
        SetNativeEvent(NativeEvent.Object, OnObjectNativeEvent);
        SetNativeEvent(NativeEvent.ObjectChange, OnObjectChangeNativeEvent);
        SetNativeEvent(NativeEvent.ObjectDelete, OnObjectDeleteNativeEvent);
        SetNativeEvent(NativeEvent.ObjectClick, OnObjectClickNativeEvent);
        SetNativeEvent(NativeEvent.WorldList, OnWorldListNativeEvent);
        SetNativeEvent(NativeEvent.WorldSetting, OnWorldSettingNativeEvent);
        SetNativeEvent(NativeEvent.WorldSettingsChanged, OnWorldSettingsChangedNativeEvent);
        SetNativeEvent(NativeEvent.Friend, OnFriendNativeEvent);
        SetNativeEvent(NativeEvent.WorldDisconnect, OnWorldDisconnectNativeEvent);
        SetNativeEvent(NativeEvent.UniverseDisconnect, OnUniverseDisconnectNativeEvent);
        SetNativeEvent(NativeEvent.UserAttributes, OnUserAttributesNativeEvent);
        SetNativeEvent(NativeEvent.QueryCellEnd, OnQueryCellEndNativeEvent);
        // SetNativeEvent(NativeEvent.TerrainNode, OnTerrainNodeNativeEvent);
        SetNativeEvent(NativeEvent.AvatarClick, OnAvatarClickNativeEvent);
        SetNativeEvent(NativeEvent.Teleport, OnTeleportNativeEvent);
        SetNativeEvent(NativeEvent.Url, OnUrlNativeEvent);
        SetNativeEvent(NativeEvent.ObjectBumpBegin, OnObjectBumpBeginNativeEvent);
        SetNativeEvent(NativeEvent.ObjectBumpEnd, OnObjectBumpEndNativeEvent);
        // SetNativeEvent(NativeEvent.TerrainNodeChanged, OnTerrainNodeChangedNativeEvent);
        SetNativeEvent(NativeEvent.Join, OnJoinNativeEvent);
        SetNativeEvent(NativeEvent.Invite, OnInviteNativeEvent);
    }

    private void SetNativeEvent(NativeEvent nativeEvent, NativeEventHandler handler)
    {
        _nativeEventHandlers.TryAdd(nativeEvent, handler);
        _ = vp_event_set(NativeInstanceHandle, nativeEvent, handler);
    }

    private void OnChatNativeEvent(nint sender)
    {
        VirtualParadiseMessage message;

        lock (Lock)
        {
            int session = vp_int(sender, IntegerAttribute.AvatarSession);
            string name = vp_string(sender, StringAttribute.AvatarName);
            string content = vp_string(sender, StringAttribute.ChatMessage);

            int type = vp_int(sender, IntegerAttribute.ChatType);

            Color color = Color.Black;
            var style = FontStyle.Regular;

            if (type == 1)
            {
                int r = vp_int(sender, IntegerAttribute.ChatRolorRed);
                int g = vp_int(sender, IntegerAttribute.ChatColorGreen);
                int b = vp_int(sender, IntegerAttribute.ChatColorBlue);
                color = Color.FromArgb(r, g, b);
                style = (FontStyle)vp_int(sender, IntegerAttribute.ChatEffects);
            }

            VirtualParadiseAvatar avatar = GetAvatar(session)!;
            message = new VirtualParadiseMessage((MessageType)type, name, content, avatar, style, color);
        }

        _messageReceived.OnNext(message);

        foreach (VirtualParadiseClientExtension extension in _extensions)
        {
            extension.OnMessageReceived(message);
        }
    }

    private async void OnAvatarAddNativeEvent(nint sender)
    {
        VirtualParadiseAvatar avatar = ExtractAvatar(sender);
        avatar = AddOrUpdateAvatar(avatar);
        avatar.User = await GetUserAsync(vp_int(sender, IntegerAttribute.UserId)).ConfigureAwait(false);
        _avatarJoined.OnNext(avatar);
    }

    private void OnAvatarChangeNativeEvent(nint sender)
    {
        int session;
        int type;
        Vector3d position;
        Rotation rotation;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);
            type = vp_int(sender, IntegerAttribute.AvatarType);

            double x = vp_double(sender, FloatAttribute.AvatarX);
            double y = vp_double(sender, FloatAttribute.AvatarY);
            double z = vp_double(sender, FloatAttribute.AvatarZ);
            position = new Vector3d(x, y, z);

            var pitch = (float)vp_double(sender, FloatAttribute.AvatarPitch);
            var yaw = (float)vp_double(sender, FloatAttribute.AvatarYaw);
            rotation = Rotation.CreateFromTiltYawRoll(pitch, yaw, 0);
        }

        VirtualParadiseAvatar avatar = GetAvatar(session)!;
        if (type != avatar.Type)
        {
            int oldType = avatar.Type;
            avatar.Type = type;

            var args = new AvatarTypeChangedEventArgs(avatar, type, oldType);
            _avatarTypeChanged.OnNext(args);
        }

        Location oldLocation = avatar.Location;
        var newLocation = new Location(oldLocation.World, position, rotation);
        avatar.Location = newLocation;

        if (position != oldLocation.Position || rotation != oldLocation.Rotation)
        {
            var args = new AvatarMovedEventArgs(avatar, newLocation, oldLocation);
            _avatarMoved.OnNext(args);
        }
    }

    private void OnAvatarDeleteNativeEvent(nint sender)
    {
        int session;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);
        }

        VirtualParadiseAvatar avatar = GetAvatar(session)!;
        _avatars.TryRemove(session, out VirtualParadiseAvatar _);
        _avatarLeft.OnNext(avatar);
    }

    private async void OnObjectNativeEvent(nint sender)
    {
        int session;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);
        }

        VirtualParadiseObject virtualParadiseObject = await ExtractObjectAsync(sender).ConfigureAwait(false);
        Cell cell = virtualParadiseObject.Location.Cell;

        virtualParadiseObject = AddOrUpdateObject(virtualParadiseObject);

        if (session == 0)
        {
            if (_cellChannels.TryGetValue(cell, out Channel<VirtualParadiseObject>? channel))
            {
                await channel.Writer.WriteAsync(virtualParadiseObject).ConfigureAwait(false);
            }
        }
        else
        {
            VirtualParadiseAvatar avatar = GetAvatar(session)!;
            var args = new ObjectCreatedEventArgs(avatar, virtualParadiseObject);
            _objectCreated.OnNext(args);
        }
    }

    private async void OnObjectChangeNativeEvent(nint sender)
    {
        int objectId;
        int session;

        lock (Lock)
        {
            objectId = vp_int(sender, IntegerAttribute.ObjectId);
            session = vp_int(sender, IntegerAttribute.AvatarSession);
        }

        VirtualParadiseAvatar avatar = GetAvatar(session)!;
        VirtualParadiseObject? cachedObject = null;

        if (_objects.TryGetValue(objectId, out VirtualParadiseObject? virtualParadiseObject))
        {
            cachedObject = await ExtractObjectAsync(sender).ConfigureAwait(false); // data discarded, but used to pull type
            cachedObject.ExtractFromOther(virtualParadiseObject);

            virtualParadiseObject.ExtractFromInstance(sender); // update existing instance
        }
        else
        {
            virtualParadiseObject = await GetObjectAsync(objectId).ConfigureAwait(false);
        }

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (virtualParadiseObject is not null)
        {
            AddOrUpdateObject(virtualParadiseObject);
        }

        var args = new ObjectChangedEventArgs(avatar, cachedObject, virtualParadiseObject!);
        _objectChanged.OnNext(args);
    }

    private async void OnObjectDeleteNativeEvent(nint sender)
    {
        int objectId;
        int session;

        lock (Lock)
        {
            objectId = vp_int(sender, IntegerAttribute.ObjectId);
            session = vp_int(sender, IntegerAttribute.AvatarSession);
        }

        VirtualParadiseAvatar? avatar = GetAvatar(session);
        VirtualParadiseObject? virtualParadiseObject;

        try
        {
            virtualParadiseObject = await GetObjectAsync(objectId).ConfigureAwait(false);
        }
        catch (VirtualParadiseException) // any exception: we don't care about GetObject failing. ID is always available
        {
            virtualParadiseObject = null;
        }

        _objects.TryRemove(objectId, out VirtualParadiseObject _);

        var args = new ObjectDeletedEventArgs(avatar!, objectId, virtualParadiseObject!);
        _objectDeleted.OnNext(args);
    }

    private async void OnObjectClickNativeEvent(nint sender)
    {
        Vector3d clickPoint;
        int objectId;
        int session;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);
            objectId = vp_int(sender, IntegerAttribute.ObjectId);

            double x = vp_double(sender, FloatAttribute.ClickHitX);
            double y = vp_double(sender, FloatAttribute.ClickHitY);
            double z = vp_double(sender, FloatAttribute.ClickHitZ);
            clickPoint = new Vector3d(x, y, z);
        }

        VirtualParadiseAvatar avatar = GetAvatar(session)!;
        try
        {
            VirtualParadiseObject virtualParadiseObject = await GetObjectAsync(objectId).ConfigureAwait(false);
            var args = new ObjectClickedEventArgs(avatar, virtualParadiseObject, clickPoint);
            _objectClicked.OnNext(args);
        }
        catch (ObjectNotFoundException)
        {
            // ignored
        }
    }

    private async void OnWorldListNativeEvent(nint sender)
    {
        VirtualParadiseWorld world;
        string name;
        int avatarCount;
        WorldState state;

        lock (Lock)
        {
            name = vp_string(sender, StringAttribute.WorldName);
            avatarCount = vp_int(sender, IntegerAttribute.WorldUsers);
            state = (WorldState)vp_int(sender, IntegerAttribute.WorldState);

            world = new VirtualParadiseWorld(this, name) { AvatarCount = avatarCount, State = state };
            _worlds[name] = world;
        }

        try
        {
            if (_worldListChannel is not null)
            {
                await _worldListChannel.Writer.WriteAsync(world).ConfigureAwait(false);
            }
        }
        catch (ChannelClosedException)
        {
            if (_worlds.TryGetValue(name, out world!))
            {
                world.AvatarCount = avatarCount;
                world.State = state;
            }
        }
    }

    private void OnWorldSettingNativeEvent(nint sender)
    {
        lock (Lock)
        {
            string key = vp_string(NativeInstanceHandle, StringAttribute.WorldSettingKey);
            string value = vp_string(NativeInstanceHandle, StringAttribute.WorldSettingValue);
            _worldSettings.TryAdd(key, value);
        }
    }

    private void OnWorldSettingsChangedNativeEvent(nint sender)
    {
        _worldSettingsCompletionSource.SetResult();
    }

    private async void OnFriendNativeEvent(nint sender)
    {
        int userId;

        lock (Lock)
        {
            userId = vp_int(sender, IntegerAttribute.FriendUserId);
        }

        VirtualParadiseUser user = await GetUserAsync(userId).ConfigureAwait(false);
        _friends.AddOrUpdate(userId, user, (_, _) => user);
    }

    private void OnWorldDisconnectNativeEvent(nint sender)
    {
        DisconnectReason reason;
        lock (Lock)
        {
            reason = (DisconnectReason)vp_int(sender, IntegerAttribute.DisconnectErrorCode);
        }

        _worldServerDisconnected.OnNext(reason);
    }

    private void OnUniverseDisconnectNativeEvent(nint sender)
    {
        DisconnectReason reason;
        lock (Lock)
        {
            reason = (DisconnectReason)vp_int(sender, IntegerAttribute.DisconnectErrorCode);
        }

        _universeServerDisconnected.OnNext(reason);
    }

    private void OnUserAttributesNativeEvent(nint sender)
    {
        int userId;
        VirtualParadiseUser user;

        lock (Lock)
        {
            userId = vp_int(sender, IntegerAttribute.UserId);
            string name = vp_string(sender, StringAttribute.UserName);
            string email = vp_string(sender, StringAttribute.UserEmail);

            int lastLogin = vp_int(sender, IntegerAttribute.UserLastLogin);
            int onlineTime = vp_int(sender, IntegerAttribute.UserOnlineTime);
            int registered = vp_int(sender, IntegerAttribute.UserRegistrationTime);

            user = new VirtualParadiseUser(this, userId)
            {
                Name = name,
                EmailAddress = email,
                LastLogin = DateTimeOffset.FromUnixTimeSeconds(lastLogin),
                OnlineTime = TimeSpan.FromSeconds(onlineTime),
                RegistrationTime = DateTimeOffset.FromUnixTimeSeconds(registered)
            };
        }

        if (_usersCompletionSources.TryGetValue(userId, out TaskCompletionSource<VirtualParadiseUser>? taskCompletionSource))
        {
            taskCompletionSource.SetResult(user);
        }
    }

    private void OnQueryCellEndNativeEvent(nint sender)
    {
        Cell cell;

        lock (Lock)
        {
            int x = vp_int(sender, IntegerAttribute.CellX);
            int z = vp_int(sender, IntegerAttribute.CellZ);

            cell = new Cell(x, z);
        }

        if (_cellChannels.TryRemove(cell, out Channel<VirtualParadiseObject>? channel))
        {
            channel.Writer.TryComplete();
        }
    }

    private void OnAvatarClickNativeEvent(nint sender)
    {
        int session, clickedSession;
        Vector3d clickPoint;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);
            clickedSession = vp_int(sender, IntegerAttribute.ClickedSession);

            double x = vp_double(sender, FloatAttribute.ClickHitX);
            double y = vp_double(sender, FloatAttribute.ClickHitY);
            double z = vp_double(sender, FloatAttribute.ClickHitZ);
            clickPoint = new Vector3d(x, y, z);
        }

        VirtualParadiseAvatar avatar = GetAvatar(session)!;
        VirtualParadiseAvatar clickedAvatar = GetAvatar(clickedSession)!;
        var args = new AvatarClickedEventArgs(avatar, clickedAvatar, clickPoint);
        _avatarClicked.OnNext(args);
    }

    private async void OnTeleportNativeEvent(nint sender)
    {
        int session;
        string worldName;
        Vector3d position;
        Rotation rotation;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);

            double x = vp_double(sender, FloatAttribute.TeleportX);
            double y = vp_double(sender, FloatAttribute.TeleportY);
            double z = vp_double(sender, FloatAttribute.TeleportZ);
            position = new Vector3d(x, y, z);

            float yaw = vp_float(sender, FloatAttribute.TeleportYaw);
            float pitch = vp_float(sender, FloatAttribute.TeleportPitch);
            rotation = Rotation.CreateFromTiltYawRoll(pitch, yaw, 0);

            worldName = vp_string(sender, StringAttribute.TeleportWorld);
        }

        VirtualParadiseWorld world = (string.IsNullOrWhiteSpace(worldName)
            ? CurrentWorld
            : await GetWorldAsync(worldName).ConfigureAwait(false))!;
        var location = new Location(world, position, rotation);

        VirtualParadiseAvatar avatar = GetAvatar(session)!;
        var args = new TeleportedEventArgs(avatar, location);
        _teleported.OnNext(args);
    }

    private async void OnObjectBumpEndNativeEvent(nint sender)
    {
        int session;
        int objectId;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);
            objectId = vp_int(sender, IntegerAttribute.ObjectId);
        }

        VirtualParadiseAvatar avatar = GetAvatar(session)!;
        try
        {
            var vpObject = await GetObjectAsync(objectId).ConfigureAwait(false);
            var args = new ObjectBumpEventArgs(avatar, vpObject, BumpPhase.End);
            _objectBump.OnNext(args);
        }
        catch (ObjectNotFoundException)
        {
            // ignored
        }
    }

    private void OnUrlNativeEvent(nint sender)
    {
        int session;
        string url;
        UriTarget target;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);
            url = vp_string(sender, StringAttribute.Url);
            target = (UriTarget)vp_int(sender, IntegerAttribute.UrlTarget);
        }

        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            return;
        }

        VirtualParadiseAvatar avatar = GetAvatar(session)!;
        var uri = new Uri(url);
        var args = new UriReceivedEventArgs(uri, target, avatar);
        _uriReceived.OnNext(args);
    }

    private async void OnObjectBumpBeginNativeEvent(nint sender)
    {
        int session;
        int objectId;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);
            objectId = vp_int(sender, IntegerAttribute.ObjectId);
        }

        VirtualParadiseAvatar avatar = GetAvatar(session)!;
        try
        {
            var vpObject = await GetObjectAsync(objectId).ConfigureAwait(false);
            var args = new ObjectBumpEventArgs(avatar, vpObject, BumpPhase.Begin);
            _objectBump.OnNext(args);
        }
        catch (ObjectNotFoundException)
        {
            // ignored
        }
    }

    private async void OnJoinNativeEvent(nint sender)
    {
        int requestId;
        int userId;
        string name;

        lock (Lock)
        {
            requestId = vp_int(NativeInstanceHandle, IntegerAttribute.JoinId);
            userId = vp_int(NativeInstanceHandle, IntegerAttribute.UserId);
            name = vp_string(NativeInstanceHandle, StringAttribute.JoinName);
        }

        VirtualParadiseUser user = await GetUserAsync(userId).ConfigureAwait(false);
        var joinRequest = new JoinRequest(this, requestId, name, user);
        _joinRequestReceived.OnNext(joinRequest);
    }

    private async void OnInviteNativeEvent(nint sender)
    {
        Vector3d position;
        Rotation rotation;
        int requestId;
        int userId;
        string worldName;
        string avatarName;

        lock (Lock)
        {
            requestId = vp_int(sender, IntegerAttribute.InviteId);
            userId = vp_int(sender, IntegerAttribute.InviteUserId);
            avatarName = vp_string(sender, StringAttribute.InviteName);

            double x = vp_double(sender, FloatAttribute.InviteX);
            double y = vp_double(sender, FloatAttribute.InviteY);
            double z = vp_double(sender, FloatAttribute.InviteZ);

            var yaw = (float)vp_double(sender, FloatAttribute.InviteYaw);
            var pitch = (float)vp_double(sender, FloatAttribute.InvitePitch);

            position = new Vector3d(x, y, z);
            rotation = Rotation.CreateFromTiltYawRoll(pitch, yaw, 0);

            worldName = vp_string(sender, StringAttribute.InviteWorld);
        }

        VirtualParadiseWorld world = (await GetWorldAsync(worldName).ConfigureAwait(false))!;
        VirtualParadiseUser user = await GetUserAsync(userId).ConfigureAwait(false);

        var location = new Location(world, position, rotation);
        var request = new InviteRequest(this, requestId, avatarName, user, location);
        _inviteRequestReceived.OnNext(request);
    }
}
