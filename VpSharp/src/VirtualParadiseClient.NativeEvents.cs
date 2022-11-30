using System.Collections.Concurrent;
using System.Drawing;
using System.Numerics;
using System.Threading.Channels;
using VpSharp.ClientExtensions;
using VpSharp.Entities;
using VpSharp.EventData;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using static VpSharp.Internal.Native;

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

            VirtualParadiseAvatar? avatar = GetAvatar(session);
            message = new VirtualParadiseMessage((MessageType)type, name, content, avatar, style, color);
        }

        var args = new MessageReceivedEventArgs(message);
        RaiseEvent(MessageReceived, args);

        foreach (VirtualParadiseClientExtension extension in _extensions)
        {
            extension.OnMessageReceived(args);
        }
    }

    private async void OnAvatarAddNativeEvent(nint sender)
    {
        VirtualParadiseAvatar avatar = ExtractAvatar(sender);
        avatar = AddOrUpdateAvatar(avatar);
        avatar.User = await GetUserAsync(vp_int(sender, IntegerAttribute.UserId)).ConfigureAwait(false);

        var args = new AvatarJoinedEventArgs(avatar);
        RaiseEvent(AvatarJoined, args);
    }

    private void OnAvatarChangeNativeEvent(nint sender)
    {
        int session;
        int type;
        Vector3d position;
        Quaternion rotation;

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
            rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0);
        }

        VirtualParadiseAvatar? avatar = GetAvatar(session);
        if (type != avatar.Type)
        {
            int oldType = avatar.Type;
            avatar.Type = type;

            var args = new AvatarTypeChangedEventArgs(avatar, type, oldType);
            RaiseEvent(AvatarTypeChanged, args);
        }

        Location oldLocation = avatar.Location;
        var newLocation = new Location(oldLocation.World, position, rotation);
        avatar.Location = newLocation;

        if (position != oldLocation.Position || rotation != oldLocation.Rotation)
        {
            var args = new AvatarMovedEventArgs(avatar, newLocation, oldLocation);
            RaiseEvent(AvatarMoved, args);
        }
    }

    private void OnAvatarDeleteNativeEvent(nint sender)
    {
        int session;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);
        }

        VirtualParadiseAvatar? avatar = GetAvatar(session);
        _avatars.TryRemove(session, out VirtualParadiseAvatar _);

        var args = new AvatarLeftEventArgs(avatar);
        RaiseEvent(AvatarLeft, args);
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
            VirtualParadiseAvatar? avatar = GetAvatar(session);
            var args = new ObjectCreatedEventArgs(avatar, virtualParadiseObject);
            RaiseEvent(ObjectCreated, args);
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

        VirtualParadiseAvatar? avatar = GetAvatar(session);
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

        var args = new ObjectChangedEventArgs(avatar, cachedObject, virtualParadiseObject);
        RaiseEvent(ObjectChanged, args);
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
        catch // any exception: we don't care about GetObject failing. ID is always available
        {
            virtualParadiseObject = null;
        }

        _objects.TryRemove(objectId, out VirtualParadiseObject? _);

        var args = new ObjectDeletedEventArgs(avatar!, objectId, virtualParadiseObject!);
        RaiseEvent(ObjectDeleted, args);
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

        VirtualParadiseAvatar? avatar = GetAvatar(session);
        VirtualParadiseObject virtualParadiseObject = await GetObjectAsync(objectId).ConfigureAwait(false);
        var args = new ObjectClickedEventArgs(avatar, virtualParadiseObject, clickPoint);
        RaiseEvent(ObjectClicked, args);
    }

    private async void OnWorldListNativeEvent(nint sender)
    {
        VirtualParadiseWorld world;

        lock (Lock)
        {
            string name = vp_string(sender, StringAttribute.WorldName);
            int avatarCount = vp_int(sender, IntegerAttribute.WorldUsers);
            var state = (WorldState)vp_int(sender, IntegerAttribute.WorldState);

            world = new VirtualParadiseWorld(this, name)
            {
                AvatarCount = avatarCount,
                State = state
            };
        }

        if (_worldListChannel is not null)
        {
            await _worldListChannel.Writer.WriteAsync(world).ConfigureAwait(false);
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

        var args = new DisconnectedEventArgs(reason);
        RaiseEvent(WorldServerDisconnected, args);
    }

    private void OnUniverseDisconnectNativeEvent(nint sender)
    {
        DisconnectReason reason;
        lock (Lock)
        {
            reason = (DisconnectReason)vp_int(sender, IntegerAttribute.DisconnectErrorCode);
        }

        var args = new DisconnectedEventArgs(reason);
        RaiseEvent(UniverseServerDisconnected, args);
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

        VirtualParadiseAvatar? avatar = GetAvatar(session);
        VirtualParadiseAvatar? clickedAvatar = GetAvatar(clickedSession);
        var args = new AvatarClickedEventArgs(avatar, clickedAvatar, clickPoint);
        RaiseEvent(AvatarClicked, args);
    }

    private async void OnTeleportNativeEvent(nint sender)
    {
        int session;
        string worldName;
        Vector3d position;
        Quaternion rotation;

        lock (Lock)
        {
            session = vp_int(sender, IntegerAttribute.AvatarSession);

            double x = vp_double(sender, FloatAttribute.TeleportX);
            double y = vp_double(sender, FloatAttribute.TeleportY);
            double z = vp_double(sender, FloatAttribute.TeleportZ);
            position = new Vector3d(x, y, z);

            float yaw = vp_float(sender, FloatAttribute.TeleportYaw);
            float pitch = vp_float(sender, FloatAttribute.TeleportPitch);
            rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0);

            worldName = vp_string(sender, StringAttribute.TeleportWorld);
        }

        VirtualParadiseWorld? world = string.IsNullOrWhiteSpace(worldName) ? CurrentWorld : await GetWorldAsync(worldName).ConfigureAwait(false);
        var location = new Location(world, position, rotation);

        VirtualParadiseAvatar? avatar = GetAvatar(session);
        var args = new TeleportedEventArgs(avatar, location);
        RaiseEvent(Teleported, args);
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

        VirtualParadiseAvatar? avatar = GetAvatar(session);
        var vpObject = await GetObjectAsync(objectId).ConfigureAwait(false);

        var args = new ObjectBumpEventArgs(avatar, vpObject, BumpPhase.End);
        RaiseEvent(ObjectBump, args);
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

        VirtualParadiseAvatar? avatar = GetAvatar(session);
        var uri = new Uri(url);
        var args = new UriReceivedEventArgs(uri, target, avatar);
        RaiseEvent(UriReceived, args);
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

        VirtualParadiseAvatar? avatar = GetAvatar(session);
        var vpObject = await GetObjectAsync(objectId).ConfigureAwait(false);

        var args = new ObjectBumpEventArgs(avatar, vpObject, BumpPhase.Begin);
        RaiseEvent(ObjectBump, args);
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
        var args = new JoinRequestReceivedEventArgs(joinRequest);
        RaiseEvent(JoinRequestReceived, args);
    }

    private async void OnInviteNativeEvent(nint sender)
    {
        Vector3d position;
        Quaternion rotation;
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
            rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0);

            worldName = vp_string(sender, StringAttribute.InviteWorld);
        }

        VirtualParadiseWorld? world = await GetWorldAsync(worldName).ConfigureAwait(false);
        VirtualParadiseUser user = await GetUserAsync(userId).ConfigureAwait(false);

        var location = new Location(world, position, rotation);
        var request = new InviteRequest(this, requestId, avatarName, user, location);
        var args = new InviteRequestReceivedEventArgs(request);
        RaiseEvent(InviteRequestReceived, args);
    }
}
