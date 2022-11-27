using System.Runtime.InteropServices;
using VpSharp.Internal.NativeAttributes;

// ReSharper disable InconsistentNaming

namespace VpSharp.Internal;

internal static class Native
{
    private const int NativeSdkVersion = 5;
    private const string VpSdkLibrary = "vpsdk";

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_init(int version = NativeSdkVersion);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr vp_create(ref NetConfig net_config);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_destroy(IntPtr instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_connect_universe(IntPtr instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string host,
        int port);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_login(IntPtr instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string username,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string password,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string botname);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_wait(IntPtr instance, int milliseconds);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_enter(IntPtr instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string world_name);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_leave(IntPtr instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_say(
        IntPtr instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string message);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_console_message(IntPtr instance,
        int session,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string name,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string message,
        int effects,
        byte red,
        byte green,
        byte blue);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_event_set(IntPtr instance,
        [MarshalAs(UnmanagedType.I4)] NativeEvent event_name,
        [MarshalAs(UnmanagedType.FunctionPtr)] NativeEventHandler @event);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_callback_set(IntPtr instance,
        [MarshalAs(UnmanagedType.I4)] NativeCallback callbackname,
        [MarshalAs(UnmanagedType.FunctionPtr)] NativeCallbackHandler callback);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_state_change(IntPtr instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_int(IntPtr instance, [MarshalAs(UnmanagedType.I4)] IntegerAttribute attr);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern float vp_float(IntPtr instance, [MarshalAs(UnmanagedType.I4)] FloatAttribute attr);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern double vp_double(IntPtr instance, [MarshalAs(UnmanagedType.I4)] FloatAttribute attr);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToManaged))]
    public static extern string vp_string(IntPtr instance, [MarshalAs(UnmanagedType.I4)] StringAttribute attr);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr vp_data(IntPtr instance, [MarshalAs(UnmanagedType.I4)] DataAttribute attr, out int length);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_int_set(IntPtr instance,
        [MarshalAs(UnmanagedType.I4)] IntegerAttribute name,
        int value);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_float_set(IntPtr instance,
        [MarshalAs(UnmanagedType.I4)] FloatAttribute name,
        float value);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_double_set(IntPtr instance,
        [MarshalAs(UnmanagedType.I4)] FloatAttribute attr,
        double value);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vp_string_set(IntPtr instance,
        StringAttribute name,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string str);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_data_set(IntPtr instance,
        [MarshalAs(UnmanagedType.I4)] DataAttribute name,
        int length,
        byte[] data);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_query_cell(IntPtr instance, int x, int z);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_query_cell_revision(IntPtr instance, int x, int z, int revision);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_add(IntPtr instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_load(IntPtr instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_bump_begin(IntPtr instance, int object_id, int session_to);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_bump_end(IntPtr instance, int object_id, int session_to);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_change(IntPtr instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_click(IntPtr instance,
        int object_id,
        int session_to,
        float hit_x,
        float hit_y,
        float hit_z);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_delete(IntPtr instance, int object_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_object_get(IntPtr instance, int object_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_world_list(IntPtr instance, int time);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_user_attributes_by_id(IntPtr instance, int user_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_user_attributes_by_name(IntPtr instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string name);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_friends_get(IntPtr instance);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_friend_add_by_name(IntPtr instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string name);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_friend_delete(IntPtr instance, int friend_user_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_terrain_query(IntPtr instance, int tile_x, int tile_z, int[,] revision);

// [DllImport(VpSdkLibrary, CallingConvention=CallingConvention.Cdecl)] public static extern int vp_terrain_node_set(IntPtr instance, int tile_x, int tile_z, int node_x, int node_z, struct vp_terrain_cell_t* cells);
    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_avatar_click(IntPtr instance, int avatar_session);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_teleport_avatar(IntPtr instance,
        int target_session,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string world,
        float x,
        float y,
        float z,
        float yaw,
        float pitch);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_url_send(IntPtr instance,
        int session_id,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string url,
        [MarshalAs(UnmanagedType.I4)] UriTarget url_target);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_join(IntPtr instance, int user_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_join_accept(IntPtr instance,
        int requestId,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string world,
        double x,
        double y,
        double z,
        float yaw,
        float pitch);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_join_decline(IntPtr instance, int requestId);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_world_permission_user_set(IntPtr instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string permission,
        int user_id,
        int enable);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_world_permission_session_set(IntPtr instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string permission,
        int session_id,
        int enable);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_world_setting_set(IntPtr instance,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string setting,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string value,
        int session_to);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_invite(IntPtr instance,
        int user_id,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringToNative))] string world,
        double x,
        double y,
        double z,
        float yaw,
        float pitch);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_invite_accept(IntPtr instance, int invitation_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_invite_decline(IntPtr instance, int invitation_id);

    [DllImport(VpSdkLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vp_net_notify(IntPtr vpConnection, int type, int status);
}