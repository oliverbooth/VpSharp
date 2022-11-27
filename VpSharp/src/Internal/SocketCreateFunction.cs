using System.Runtime.InteropServices;

namespace VpSharp.Internal;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate IntPtr SocketCreateFunction(IntPtr connection, IntPtr context);