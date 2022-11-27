namespace VpSharp;

/// <summary>
///     An enumeration of reasons for a disconnect.
/// </summary>
public enum DisconnectReason
{
    /// <summary>
    ///     Indicates that connection to the server was lost unexpectedly.
    /// </summary>
    ConnectionLost,
        
    /// <summary>
    ///     Indicates that disconnection was requested and graceful.
    /// </summary>
    Disconnected
}