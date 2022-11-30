using System.Runtime.InteropServices;
using VpSharp.Exceptions;
using VpSharp.Internal;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private GCHandle _instanceHandle;
    private NetConfig _netConfig;

    internal object Lock { get; } = new();

    internal nint NativeInstanceHandle { get; private set; }

    private void Initialize()
    {
        var reason = (ReasonCode) Native.vp_init();
        if (reason == ReasonCode.VersionMismatch)
        {
            throw new VersionMismatchException();
        }

        _instanceHandle = GCHandle.Alloc(this);
        _netConfig.Context = GCHandle.ToIntPtr(_instanceHandle);
        _netConfig.Create = Connection.CreateNative;
        _netConfig.Destroy = Connection.DestroyNative;
        _netConfig.Connect = Connection.ConnectNative;
        _netConfig.Receive = Connection.ReceiveNative;
        _netConfig.Send = Connection.SendNative;
        _netConfig.Timeout = Connection.TimeoutNative;

        NativeInstanceHandle = Native.vp_create(ref _netConfig);

        SetNativeEvents();
        SetNativeCallbacks();
    }
}
