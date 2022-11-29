using System.Runtime.InteropServices;

namespace VpSharp.Internal;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct NetConfig
{
    public SocketCreateFunction Create;
    public SocketDestroyFunction Destroy;
    public SocketConnectFunction Connect;
    public SocketSendFunction Send;
    public SocketReceiveFunction Receive;
    public SocketTimeoutFunction Timeout;
    public SocketWaitFunction Wait;
    public nint Context;
}
