using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event data for <see cref="VirtualParadiseClient.UriReceived" />.
/// </summary>
public sealed class UriReceivedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UriReceivedEventArgs" /> class.
    /// </summary>
    /// <param name="uri">The received URI.</param>
    /// <param name="target">The URI target.</param>
    /// <param name="avatar">The avatar who sent the URI.</param>
    public UriReceivedEventArgs(Uri uri, UriTarget target, VirtualParadiseAvatar avatar)
    {
        Uri = uri;
        Target = target;
        Avatar = avatar;
    }

    /// <summary>
    ///     Gets the avatar who sent the URI.
    /// </summary>
    /// <value>The avatar who sent the URI.</value>
    public VirtualParadiseAvatar Avatar { get; }

    /// <summary>
    ///     Gets the URI target.
    /// </summary>
    /// <value>A value from the <see cref="UriTarget" /> enum.</value>
    public UriTarget Target { get; }

    /// <summary>
    ///     Gets the URI which was received.
    /// </summary>
    /// <value>The received URI.</value>
    public Uri Uri { get; }
}