using System.Runtime.InteropServices;

namespace VpSharp.Internal;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate nint SocketCreateFunction(nint connection, nint context);
