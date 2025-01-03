using System.Runtime.InteropServices;

namespace VpSharp.Internal;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate int SocketTimeoutFunction(nint socket, int seconds);
