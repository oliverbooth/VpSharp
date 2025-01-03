using System.Runtime.InteropServices;

namespace VpSharp.Internal;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate int SocketConnectFunction(nint socket, nint host, ushort port);
