using System.Runtime.InteropServices;
using System.Text;

namespace VpSharp.Internal;

internal sealed class Utf8StringToNative : ICustomMarshaler
{
    private static readonly Encoding Utf8Encoding = Encoding.UTF8;
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
        int byteCount = Utf8Encoding.GetByteCount(managedString);
        Span<byte> utf8Bytes = stackalloc byte[byteCount];
        Utf8Encoding.GetBytes(managedString, utf8Bytes);

        fixed (byte* data = &MemoryMarshal.GetReference(utf8Bytes))
        {
            nint buffer = Marshal.AllocHGlobal(byteCount + 1);
            Buffer.MemoryCopy(data, (void*)buffer, byteCount, byteCount);
            Marshal.WriteByte(buffer, byteCount, 0);
            return buffer;
        }
    }

    public object MarshalNativeToManaged(nint pNativeData)
    {
        throw new NotImplementedException();
    }
}
