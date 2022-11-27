namespace VpSharp;

/// <summary>
///     An enumeration of invite responses.
/// </summary>
public enum InviteResponse
{
    /// <summary>
    ///     The invite request was accepted by the user.
    /// </summary>
    Accepted,

    /// <summary>
    ///     The invite request was declined by the user.
    /// </summary>
    Declined,

    /// <summary>
    ///     The invite request timed out.
    /// </summary>
    TimeOut
}