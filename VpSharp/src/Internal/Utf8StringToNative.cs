using System.Runtime.InteropServices;
using System.Text;

namespace VpSharp.Internal;

internal sealed class Utf8StringToNative : ICustomMarshaler
{
    private static Utf8StringToNative? s_instance;

    public static ICustomMarshaler GetInstance(string cookie)
    {
        return s_instance ??= new Utf8StringToNative();
    }

    public void CleanUpManagedData(object managedObj)
    {
    }

    public void CleanUpNativeData(nint pNativeData)
    {
        Marshal.FreeHGlobal(pNativeData);
    }

    public int GetNativeDataSize()
    {
        return -1;
    }

    public unsafe nint MarshalManagedToNative(object managedObj)
    {
        var managedString = (string)managedObj;
        Span<byte> utf8Bytes = stackalloc byte[managedString.Length];
        Encoding.UTF8.GetBytes(managedString, utf8Bytes);

        fixed (byte* data = &MemoryMarshal.GetReference(utf8Bytes))
        {
            nint buffer = Marshal.AllocHGlobal(utf8Bytes.Length + 1);
            Buffer.MemoryCopy(data, (void*)buffer, utf8Bytes.Length, utf8Bytes.Length);
            Marshal.WriteByte(buffer, utf8Bytes.Length, 0);
            return buffer;
        }
    }

    public object MarshalNativeToManaged(nint pNativeData)
    {
        throw new NotImplementedException();
    }
}
