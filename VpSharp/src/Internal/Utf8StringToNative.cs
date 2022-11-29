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

    public nint MarshalManagedToNative(object managedObj)
    {
        byte[] utf8Data = Encoding.UTF8.GetBytes((string)managedObj);
        nint buffer = Marshal.AllocHGlobal(utf8Data.Length + 1);
        Marshal.Copy(utf8Data, 0, buffer, utf8Data.Length);
        Marshal.WriteByte(buffer, utf8Data.Length, 0);
        return buffer;
    }

    public object MarshalNativeToManaged(nint pNativeData)
    {
        throw new NotImplementedException();
    }
}