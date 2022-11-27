using System.Runtime.InteropServices;

namespace VpSharp.Internal;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void NativeCallbackHandler(IntPtr sender, [MarshalAs(UnmanagedType.I4)] ReasonCode reason, int reference);