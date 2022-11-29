using System.Runtime.InteropServices;

namespace VpSharp.Internal;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void NativeCallbackHandler(nint sender, [MarshalAs(UnmanagedType.I4)] ReasonCode reason, int reference);