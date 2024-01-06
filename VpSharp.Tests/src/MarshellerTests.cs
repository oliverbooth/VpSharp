using System.Diagnostics;
using System.Text;
using VpSharp.Internal;

namespace VpSharp.Tests;

[TestClass]
public class MarshellerTests
{
    private static readonly Utf8StringToNative ManagedToNativeMarshaller = new();
    private static readonly Utf8StringToManaged NativeToManagedMarshaller = new();
    private static readonly Random Random = new();

    [TestMethod]
    public unsafe void MarshalNativeToManaged_ShouldReturnPointerToUtf8Bytes_GivenString()
    {
        string value = GenerateRandomString();
        byte* pointer = GetBytePointer(value, out int count);
        string expected = Encoding.UTF8.GetString(pointer, count);
        var actual = (string)NativeToManagedMarshaller.MarshalNativeToManaged((nint)pointer);
        Assert.AreEqual(expected, actual);

        CleanUpNativeData(pointer);
    }

    [TestMethod]
    public unsafe void MarshalManagedToNative_ShouldReturnPointerToUtf8Bytes_GivenString()
    {
        string value = GenerateRandomString();
        byte* pointer = GetBytePointer(value, out int count);
        string result = Encoding.UTF8.GetString(pointer, count);

        Trace.WriteLine($"Test string: {value}");
        Assert.AreEqual(value, result);

        CleanUpNativeData(pointer);
    }

    private static unsafe void CleanUpNativeData(byte* pointer)
    {
        ManagedToNativeMarshaller.CleanUpNativeData((nint)pointer);
    }

    private static string GenerateRandomString(int length = 50)
    {
        var builder = new StringBuilder();

        for (var index = 0; index < length; index++)
        {
            int mode = Random.Next(0, 3);
            switch (mode)
            {
                case 0:
                    builder.Append((char)Random.Next('A', 'Z'));
                    break;
                case 1:
                    builder.Append((char)Random.Next('a', 'z'));
                    break;
                default:
                    builder.Append((char)Random.Next('0', '9'));
                    break;
            }
        }

        return builder.ToString();
    }

    private static unsafe byte* GetBytePointer(string input, out int byteCount)
    {
        var marshaller = new Utf8StringToNative();
        nint buffer = marshaller.MarshalManagedToNative(input);
        byteCount = Encoding.UTF8.GetByteCount(input);
        return (byte*)buffer;
    }
}
