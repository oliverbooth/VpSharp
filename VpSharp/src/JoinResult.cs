namespace VpSharp;

/// <summary>
///     Represents a join result.
/// </summary>
#pragma warning disable CA1815
public readonly struct JoinResult
#pragma warning restore CA1815
{
    internal JoinResult(JoinResponse response, Location? location)
    {
        Response = response;
        Location = location;
    }

    /// <summary>
    ///     Gets the join location.
    /// </summary>
    /// <value>The join location. This value will be <see langword="null" /> if the request was declined, or timed out.</value>
    public Location? Location { get; }

    /// <summary>
    ///     Gets the join response.
    /// </summary>
    /// <value>The join response.</value>
    public JoinResponse Response { get; }
}
