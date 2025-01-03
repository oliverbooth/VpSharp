using System.Runtime.InteropServices;

namespace VpSharp.Internal;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate int SocketWaitFunction(nint context, int duration);
