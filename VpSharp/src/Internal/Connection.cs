using System.Net.Sockets;
using System.Runtime.InteropServices;
using VpSharp.NativeApi;

namespace VpSharp.Internal;

internal sealed class Connection : IDisposable
{
    private readonly object _lockObject;
    private readonly Socket _socket;

    private byte[] _pendingBuffer;
    private readonly List<byte[]> _readyBuffers = new();
    private Timer _timer;
    private nint _vpConnection;

    public Connection(nint vpConnection, object lockObject)
    {
        _vpConnection = vpConnection;
        _lockObject = lockObject;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void BeforeDestroy()
    {
        // ReSharper disable once InconsistentlySynchronizedField
        _vpConnection = nint.Zero;
    }

    public int Connect(string host, ushort port)
    {
        _socket.BeginConnect(host, port, ConnectCallback, this);
        return (int)NetworkReturnCode.Success;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _socket.Dispose();
    }

    private static void ConnectCallback(IAsyncResult ar)
    {
        var connection = ar.AsyncState as Connection;
        try
        {
            if (connection is not null)
            {
                connection._socket.EndConnect(ar);

                connection._pendingBuffer = new byte[1024];
                connection._socket.BeginReceive(connection._pendingBuffer, 0, 1024, SocketFlags.None, ReceiveCallback,
                    connection);

                connection.Notify(NetworkNotification.Connect, (int) NetworkReturnCode.Success);
            }
        }
        catch (SocketException)
        {
            connection?.Notify(NetworkNotification.Connect, (int) NetworkReturnCode.ConnectionError);
        }
    }

    public static int ConnectNative(nint ptr, nint hostPtr, ushort port)
    {
        GCHandle handle = GCHandle.FromIntPtr(ptr);
        var connection = handle.Target as Connection;
        string host = Marshal.PtrToStringAnsi(hostPtr);
        if (connection is not null)
        {
            return connection.Connect(host, port);
        }

        return 0;
    }

    public static nint CreateNative(nint vpConnection, nint context)
    {
        GCHandle contextHandle = GCHandle.FromIntPtr(context);
        var connection = new Connection(vpConnection, contextHandle.Target);
        GCHandle handle = GCHandle.Alloc(connection, GCHandleType.Normal);
        var ptr = GCHandle.ToIntPtr(handle);
        return ptr;
    }

    public static void DestroyNative(nint ptr)
    {
        GCHandle handle = GCHandle.FromIntPtr(ptr);
        var connection = handle.Target as Connection;
        connection?.BeforeDestroy();
        handle.Free();
        connection?.Dispose();
    }

    private void HandleTimeout()
    {
        if (_timer != null)
        {
            Notify(NetworkNotification.Timeout, 0);
        }
    }

    private static void HandleTimeout(object? state)
    {
        (state as Connection)?.HandleTimeout();
    }

    private void Notify(NetworkNotification notification, int rc)
    {
        lock (_lockObject)
        {
            if (_vpConnection != IntPtr.Zero)
            {
                Native.vp_net_notify(_vpConnection, (int) notification, rc);
            }
        }
    }

    public int Receive(nint data, uint length)
    {
        if (_readyBuffers.Count == 0)
        {
            return (int)NetworkReturnCode.WouldBlock;
        }

        var spaceLeft = (int)length;
        nint destination = data;

        int i;
        for (i = 0; i < _readyBuffers.Count && spaceLeft > 0; ++i)
        {
            byte[] source = _readyBuffers[i];
            if (source.Length > spaceLeft)
            {
                Marshal.Copy(source, 0, data, spaceLeft);

                _readyBuffers[i] = new byte[source.Length - spaceLeft];
                Array.Copy(source, spaceLeft, _readyBuffers[i], 0, source.Length - spaceLeft);

                spaceLeft = 0;
                break;
            }

            Marshal.Copy(source, 0, destination, source.Length);
            destination += source.Length;
            spaceLeft -= source.Length;
        }

        _readyBuffers.RemoveRange(0, i);

        return (int)(length - spaceLeft);
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        if (ar.AsyncState is not Connection connection)
        {
            return;
        }

        int bytesRead;

        try
        {
            bytesRead = connection._socket.EndReceive(ar);
        }
        catch (SocketException e)
        {
            connection.Notify(NetworkNotification.Disconnect, e.ErrorCode);
            return;
        }

        if (bytesRead < connection._pendingBuffer.Length)
        {
            var buffer = new byte[bytesRead];
            Array.Copy(connection._pendingBuffer, buffer, bytesRead);
            connection._readyBuffers.Add(buffer);
        }
        else
        {
            connection._readyBuffers.Add(connection._pendingBuffer);
            connection._pendingBuffer = new byte[1024];
        }

        if (connection._vpConnection != nint.Zero)
        {
            if (bytesRead > 0)
            {
                connection.Notify(NetworkNotification.ReadReady, 0);

                try
                {
                    connection._socket.BeginReceive(connection._pendingBuffer, 0, 1024, SocketFlags.None, ReceiveCallback,
                        connection);
                }
                catch (SocketException e)
                {
                    connection.Notify(NetworkNotification.Disconnect, e.ErrorCode);
                }
            }
            else
            {
                connection.Notify(NetworkNotification.Disconnect, 0);
            }
        }
    }

    public static int ReceiveNative(nint ptr, nint data, uint length)
    {
        if (GCHandle.FromIntPtr(ptr).Target is Connection connection)
        {
            return connection.Receive(data, length);
        }

        return 0;
    }

    public int Send(nint data, uint length)
    {
        var buffer = new byte[length];
        Marshal.Copy(data, buffer, 0, (int)length);
        try
        {
            return _socket.Send(buffer);
        }
        catch (SocketException)
        {
            return -1;
        }
    }

    public static int SendNative(nint ptr, nint data, uint length)
    {
        if (GCHandle.FromIntPtr(ptr).Target is Connection connection)
        {
            return connection.Send(data, length);
        }

        return 0;
    }

    public int Timeout(int seconds)
    {
        if (seconds < 0)
        {
            _timer = null;
        }
        else
        {
            _timer = new Timer(HandleTimeout, this, seconds * 1000, global::System.Threading.Timeout.Infinite);
        }

        return 0;
    }

    public static int TimeoutNative(IntPtr ptr, int seconds)
    {
        if (GCHandle.FromIntPtr(ptr).Target is Connection connection)
        {
            return connection.Timeout(seconds);
        }

        return 0;
    }
}
