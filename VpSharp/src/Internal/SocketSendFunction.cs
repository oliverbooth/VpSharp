using System.Runtime.InteropServices;

namespace VpSharp.Internal;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate int SocketSendFunction(nint socket, nint data, uint length);
