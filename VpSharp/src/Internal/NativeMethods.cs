using System.Runtime.InteropServices;
using VpSharp.Internal.NativeAttributes;

// ReSharper disable InconsistentNaming

namespace VpSharp.Internal;

internal static class NativeMethods
{
    private const int NativeSdkVersion = 5;
    private const string VpSdkLibrary = "vpsdk";

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_init(int version = NativeSdkVersion);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint vp_create(ref NetConfig net_config);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_destroy(nint instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_connect_universe(nint instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string host,
        int port);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_login(nint instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string username,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string password,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string botname);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_wait(nint instance, int milliseconds);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_enter(nint instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string world_name);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_leave(nint instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_say(
        nint instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string message);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_console_message(nint instance,
        int session,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string name,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string message,
        int effects,
        byte red,
        byte green,
        byte blue);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_event_set(nint instance,
        [MarshalAs(UnmanagedType.I4)] NativeEvent event_name,
        [MarshalAs(UnmanagedType.FunctionPtr)] NativeEventHandler @event);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_callback_set(nint instance,
        [MarshalAs(UnmanagedType.I4)] NativeCallback callbackname,
        [MarshalAs(UnmanagedType.FunctionPtr)] NativeCallbackHandler callback);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_state_change(nint instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_int(nint instance, [MarshalAs(UnmanagedType.I4)] IntegerAttribute attr);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern float vp_float(nint instance, [MarshalAs(UnmanagedType.I4)] FloatAttribute attr);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern double vp_double(nint instance, [MarshalAs(UnmanagedType.I4)] FloatAttribute attr);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToManaged))]
    public static extern string vp_string(nint instance, [MarshalAs(UnmanagedType.I4)] StringAttribute attr);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint vp_data(nint instance, [MarshalAs(UnmanagedType.I4)] DataAttribute attr, out int length);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_int_set(nint instance,
        [MarshalAs(UnmanagedType.I4)] IntegerAttribute name,
        int value);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_float_set(nint instance,
        [MarshalAs(UnmanagedType.I4)] FloatAttribute name,
        float value);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_double_set(nint instance,
        [MarshalAs(UnmanagedType.I4)] FloatAttribute attr,
        double value);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vp_string_set(nint instance,
        StringAttribute name,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string str);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_data_set(nint instance,
        [MarshalAs(UnmanagedType.I4)] DataAttribute name,
        int length,
        byte[] data);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_query_cell(nint instance, int x, int z);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_query_cell_revision(nint instance, int x, int z, int revision);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_add(nint instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_load(nint instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_bump_begin(nint instance, int object_id, int session_to);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_bump_end(nint instance, int object_id, int session_to);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_change(nint instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_click(nint instance,
        int object_id,
        int session_to,
        float hit_x,
        float hit_y,
        float hit_z);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_delete(nint instance, int object_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_get(nint instance, int object_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_world_list(nint instance, int time);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_user_attributes_by_id(nint instance, int user_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_user_attributes_by_name(nint instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string name);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_friends_get(nint instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_friend_add_by_name(nint instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string name);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_friend_delete(nint instance, int friend_user_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_terrain_query(nint instance, int tile_x, int tile_z, int[,] revision);

// [DllImport(VpSdkLibrary, CallingConvention=CallingConvention.Cdecl)] public static extern int vp_terrain_node_set(IntPtr instance, int tile_x, int tile_z, int node_x, int node_z, struct vp_terrain_cell_t* cells);
    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_avatar_click(nint instance, int avatar_session);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_teleport_avatar(nint instance,
        int target_session,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string world,
        float x,
        float y,
        float z,
        float yaw,
        float pitch);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_url_send(nint instance,
        int session_id,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string url,
        [MarshalAs(UnmanagedType.I4)] UriTarget url_target);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_join(nint instance, int user_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_join_accept(nint instance,
        int requestId,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string world,
        double x,
        double y,
        double z,
        float yaw,
        float pitch);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_join_decline(nint instance, int requestId);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_world_permission_user_set(nint instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string permission,
        int user_id,
        int enable);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_world_permission_session_set(nint instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string permission,
        int session_id,
        int enable);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_world_setting_set(nint instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string setting,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string value,
        int session_to);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_invite(nint instance,
        int user_id,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string world,
        double x,
        double y,
        double z,
        float yaw,
        float pitch);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_invite_accept(nint instance, int invitation_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_invite_decline(nint instance, int invitation_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_net_notify(nint vpConnection, int type, int status);
}
