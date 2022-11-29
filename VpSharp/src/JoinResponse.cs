namespace VpSharp;

/// <summary>
///     An enumeration of join responses.
/// </summary>
public enum JoinResponse
{
    /// <summary>
    ///     The join request was accepted by the user.
    /// </summary>
    Accepted,

    /// <summary>
    ///     The join request was declined by the user.
    /// </summary>
    Declined,

    /// <summary>
    ///     The join request timed out.
    /// </summary>
    TimeOut
}
