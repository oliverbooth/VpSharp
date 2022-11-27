﻿using System.Collections.Concurrent;
using VpSharp.Entities;
using VpSharp.Internal;
using static VpSharp.Internal.Native;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly ConcurrentDictionary<NativeCallback, NativeCallbackHandler> _nativeCallbackHandlers = new();

    private void SetNativeCallbacks()
    {
        // SetNativeCallback(NativeCallback.ObjectAdd, OnObjectAddNativeCallback);
        // SetNativeCallback(NativeCallback.ObjectChange, OnObjectChangeNativeCallback);
        // SetNativeCallback(NativeCallback.ObjectDelete, OnObjectDeleteNativeCallback);
        // SetNativeCallback(NativeCallback.GetFriends, OnGetFriendsNativeCallback);
        // SetNativeCallback(NativeCallback.FriendAdd, OnFriendAddNativeCallback);
        // SetNativeCallback(NativeCallback.FriendDelete, OnFriendDeleteNativeCallback);
        // SetNativeCallback(NativeCallback.TerrainQuery, OnTerrainQueryNativeCallback);
        // SetNativeCallback(NativeCallback.TerrainNodeSet, OnTerrainNodeSetNativeCallback);
        SetNativeCallback(NativeCallback.ObjectGet, OnObjectGetNativeCallback);
        // SetNativeCallback(NativeCallback.ObjectLoad, OnObjectLoadNativeCallback);
        SetNativeCallback(NativeCallback.Login, OnLoginNativeCallback);
        SetNativeCallback(NativeCallback.Enter, OnEnterNativeCallback);
        // SetNativeCallback(NativeCallback.Join, OnJoinNativeCallback);
        SetNativeCallback(NativeCallback.ConnectUniverse, OnConnectUniverseNativeCallback);
        // SetNativeCallback(NativeCallback.WorldPermissionUserSet, OnWorldPermissionUserSetNativeCallback);
        // SetNativeCallback(NativeCallback.WorldPermissionSessionSet, OnWorldPermissionSessionSetNativeCallback);
        // SetNativeCallback(NativeCallback.WorldSettingSet, OnWorldSettingSetNativeCallback);
        // SetNativeCallback(NativeCallback.Invite, OnInviteNativeCallback);
        SetNativeCallback(NativeCallback.WorldList, OnWorldListNativeCallback);
    }

    private void SetNativeCallback(NativeCallback nativeCallback, NativeCallbackHandler handler)
    {
        _nativeCallbackHandlers.TryAdd(nativeCallback, handler);
        vp_callback_set(NativeInstanceHandle, nativeCallback, handler);
    }

    private async void OnObjectGetNativeCallback(IntPtr sender, ReasonCode reason, int reference)
    {
        if (!_objectCompletionSources.TryGetValue(reference, out TaskCompletionSource<(ReasonCode, VirtualParadiseObject)>? taskCompletionSource))
        {
            return;
        }

        VirtualParadiseObject virtualParadiseObject = reason == ReasonCode.Success ? await ExtractObjectAsync(sender) : null;
        taskCompletionSource.SetResult((reason, virtualParadiseObject));
    }

    private void OnLoginNativeCallback(IntPtr sender, ReasonCode reason, int reference)
    {
        _loginCompletionSource?.SetResult(reason);
    }

    private void OnEnterNativeCallback(IntPtr sender, ReasonCode reason, int reference)
    {
        _enterCompletionSource?.SetResult(reason);
    }

    private void OnConnectUniverseNativeCallback(IntPtr sender, ReasonCode reason, int reference)
    {
        _connectCompletionSource?.SetResult(reason);
    }

    private void OnWorldListNativeCallback(IntPtr sender, ReasonCode reason, int reference)
    {
        _worldListChannel?.Writer.Complete();
    }
}
